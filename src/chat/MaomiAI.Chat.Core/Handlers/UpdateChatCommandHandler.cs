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

public class UpdateChatCommandHandler : IRequestHandler<UpdateChatCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IRedisDatabase _redisDatabase;
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    public UpdateChatCommandHandler(DatabaseContext databaseContext, IRedisDatabase redisDatabase, IMediator mediator, UserContext userContext)
    {
        _databaseContext = databaseContext;
        _redisDatabase = redisDatabase;
        _mediator = mediator;
        _userContext = userContext;
    }

    public async Task<EmptyCommandResponse> Handle(UpdateChatCommand request, CancellationToken cancellationToken)
    {
        var chatEntity = await _databaseContext.UserChats
            .Where(x => x.Id == request.ChatId)
            .FirstOrDefaultAsync(cancellationToken);

        if (chatEntity==null)
        {
            throw new BusinessException("对话不存在或已被删除");
        }

        // 重新检查用户是否为团队成员
        var isTeamMember = await _mediator.Send(new QueryUserIsTeamMemberCommand
        {
            TeamId = chatEntity.TeamId,
            UserId = _userContext.UserId
        });

        if (isTeamMember.IsMember == false)
        {
            // todo: 如果用户不再是团队成员，则无法更新对话，但是可以读取对话?
            throw new BusinessException("不是该团队成员，无法更新对话");
        }

        if (request.ModelId != chatEntity.ModelId)
        {
            var model = await _databaseContext.TeamAiModels
                .Where(x => x.Id == request.ModelId && x.TeamId == chatEntity.TeamId).AnyAsync();

            if (model == false)
            {
                throw new BusinessException("该团队没有选择的 AI 模型");
            }

            chatEntity.ModelId = request.ModelId;
        }

        if (request.WikiId != chatEntity.WikiId)
        {
            if (request.WikiId != null && request.WikiId != Guid.Empty)
            {
                // 检查知识库是否存在
                var wiki = await _databaseContext.TeamWikis
                    .Where(x => x.Id == request.WikiId && (x.TeamId == chatEntity.TeamId || x.IsPublic)).AnyAsync();
                if (!wiki)
                {
                    throw new BusinessException("选择的知识库不存在或不在该团队中");
                }
            }

            chatEntity.WikiId = request.WikiId ?? Guid.Empty;
        }

        var pluginIds = request.PluginIds?.ToHashSet() ?? new HashSet<Guid>();

        // 检查插件是否存在
        if (pluginIds.Count != 0)
        {
            // 如果出现该团队中找不到的插件
            var pluginCount = await _databaseContext.TeamPlugins
                .Where(x => x.TeamId == chatEntity.TeamId && pluginIds.Contains(x.Id)).CountAsync();
            if (pluginCount != pluginIds.Count)
            {
                throw new BusinessException("选择的插件不存在或不在该团队中");
            }

            chatEntity.PluginIds = DBJsonHelper.ToJsonString(pluginIds);
        }

        _databaseContext.UserChats.Update(chatEntity);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        await _redisDatabase.Database.HashSetAsync(
            key: ChatKeyHelper.GetChatKey(chatEntity.Id),
            hashFields: request.ChatHistory.Select(x => new HashEntry(x.ModelId, x.ToRedisValue())).ToArray());

        return EmptyCommandResponse.Default;
    }
}
