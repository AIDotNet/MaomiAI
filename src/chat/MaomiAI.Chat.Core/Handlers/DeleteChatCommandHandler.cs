// <copyright file="Class1.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Chat.Shared.Commands.Responses;
using MaomiAI.Chat.Shared.Helpers;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Database.Helper;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MaomiAI.Chat.Core.Handlers;


/// <summary>
/// 删除话题.
/// </summary>
public class DeleteChatCommandHandler : IRequestHandler<DeleteChatCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IRedisDatabase _redisDatabase;
    private readonly UserContext _userContext;
    public DeleteChatCommandHandler(DatabaseContext databaseContext, IRedisDatabase redisDatabase, UserContext userContext)
    {
        _databaseContext = databaseContext;
        _redisDatabase = redisDatabase;
        _userContext = userContext;
    }
    public async Task<EmptyCommandResponse> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        var chatEntity = await _databaseContext.UserChats
            .Where(x => x.Id == request.ChatId && x.CreateUserId == _userContext.UserId)
            .AnyAsync(cancellationToken);

        if (chatEntity == false)
        {
            throw new BusinessException("对话不存在或没有权限删除");
        }

        await _databaseContext.UserChats.Where(x => x.Id == request.ChatId && x.CreateUserId == _userContext.UserId)
            .ExecuteUpdateAsync(x => x.SetProperty(a => a.IsDeleted, false));

        await _redisDatabase.Database.KeyDeleteAsync(ChatKeyHelper.GetChatKey(request.ChatId), cancellationToken);
        return EmptyCommandResponse.Default;
    }
}
