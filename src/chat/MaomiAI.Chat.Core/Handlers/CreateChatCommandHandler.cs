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

// todo: 个人助手信息获取和设置

// 创建对话

public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, CreateChatCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IRedisDatabase _redisDatabase;
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    public CreateChatCommandHandler(DatabaseContext databaseContext, IRedisDatabase redisDatabase, IMediator mediator, UserContext userContext)
    {
        _databaseContext = databaseContext;
        _redisDatabase = redisDatabase;
        _mediator = mediator;
        _userContext = userContext;
    }

    public async Task<CreateChatCommandResponse> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        // 用户是否为团队成员
        var isTeamMember = await _mediator.Send(new QueryUserIsTeamMemberCommand
        {
            TeamId = request.TeamId,
            UserId = _userContext.UserId
        });

        if (isTeamMember.IsMember == false)
        {
            throw new BusinessException("不是该团队成员，无法创建对话");
        }

        // 检查团队有没有该模型
        var model = await _databaseContext.TeamAiModels
            .Where(x => x.Id == request.ModelId && x.TeamId == request.TeamId).AnyAsync();

        if (model == false)
        {
            throw new BusinessException("该团队没有选择的 AI 模型");
        }

        if (request.WikiId != null && request.WikiId != Guid.Empty)
        {
            // 检查知识库是否存在
            var wiki = await _databaseContext.TeamWikis
                .Where(x => x.Id == request.WikiId && (x.TeamId == request.TeamId || x.IsPublic)).AnyAsync();
            if (!wiki)
            {
                throw new BusinessException("选择的知识库不存在或不在该团队中");
            }
        }

        var pluginIds = request.PluginIds?.ToHashSet() ?? new HashSet<Guid>();

        // 检查插件是否存在
        if (pluginIds.Count != 0)
        {
            // 如果出现该团队中找不到的插件
            var pluginCount = await _databaseContext.TeamPlugins
                .Where(x => x.TeamId == request.TeamId && pluginIds.Contains(x.Id)).CountAsync();
            if (pluginCount != pluginIds.Count)
            {
                throw new BusinessException("选择的插件不存在或不在该团队中");
            }
        }

        var chatEntity = new UserChatEntity
        {
            Id = Guid.NewGuid(),
            TeamId = request.TeamId,
            ModelId = request.ModelId,
            WikiId = request.WikiId ?? Guid.Empty,
            PluginIds = DBJsonHelper.ToJsonString(pluginIds),
            ExecutionSettings = DBJsonHelper.ToJsonString(request.ExecutionSettings),
            Title = request.Title
        };

        await _databaseContext.UserChats.AddAsync(chatEntity, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        await _redisDatabase.Database.HashSetAsync(
            key: ChatKeyHelper.GetChatKey(chatEntity.Id),
            hashFields: request.ChatHistory.Select(x => new HashEntry(x.ModelId, x.ToRedisValue())).ToArray());

        return new CreateChatCommandResponse
        {
            ChatId = chatEntity.Id
        };
    }
}
