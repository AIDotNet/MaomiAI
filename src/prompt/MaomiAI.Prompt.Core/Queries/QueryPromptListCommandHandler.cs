// <copyright file="QueryPromptListCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Infra;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Helpers;
using MaomiAI.Infra.Models;
using MaomiAI.Prompt.Models;
using MaomiAI.Prompt.Queries;
using MaomiAI.Prompt.Queries.Responses;
using MaomiAI.Store.Queries;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Prompt.Core.Queries;

public class QueryPromptListCommandHandler : IRequestHandler<QueryPromptListCommand, QueryPromptListCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly UserContext _userContext;
    private readonly IMediator _mediator;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryPromptListCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="userContext"></param>
    /// <param name="mediator"></param>
    public QueryPromptListCommandHandler(DatabaseContext databaseContext, UserContext userContext, IMediator mediator, SystemOptions systemOptions)
    {
        _databaseContext = databaseContext;
        _userContext = userContext;
        _mediator = mediator;
        _systemOptions = systemOptions;
    }

    /// <inheritdoc/>
    public async Task<QueryPromptListCommandResponse> Handle(QueryPromptListCommand request, CancellationToken cancellationToken)
    {
        int count = 0;
        var query = _databaseContext.Prompts.AsQueryable();

        if (request.TeamId != null)
        {
            query = query.Where(x => x.TeamId == request.TeamId);
        }
        else
        {
            query = query.Where(x => x.IsPublic || _databaseContext.Teams.Any(a => a.Id == x.TeamId && a.OwnerId == _userContext.UserId) || _databaseContext.TeamMembers.Where(a => a.Id == x.TeamId && a.UserId == _userContext.UserId).Any());
        }

        if (request.PromptType != null)
        {
            query = query.Where(x => x.Type == request.PromptType.ToString());
        }

        List<PromptItem> promptItems = new();

        if (request.PromptId != null)
        {
            // 检查该提示词是公开的，如果不公开则检查用户是否在团队下
            var resultQuery = query
                .Where(x => x.Id == request.PromptId);

            count = await resultQuery.CountAsync();

            var result = await resultQuery
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Tags,
                    x.Content,
                    x.AvatarPath,
                    x.CreateTime,
                    x.UpdateTime,
                    x.CreateUserId,
                    x.Type,
                    x.TeamId,
                    TeamName = _databaseContext.Teams.Where(a => a.Id == x.TeamId).Select(a => a.Name).FirstOrDefault()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new BusinessException("提示词不存在") { StatusCode = 404 };
            }

            promptItems.Add(new PromptItem
            {
                Id = result.Id,
                TeamId = result.TeamId,
                TeamName = result.TeamName ?? string.Empty,
                Name = result.Name,
                Description = result.Description,
                Content = result.Content,
                Tags = result.Tags.Split(',').ToList(),
                AvatarPath = result.AvatarPath,
                PromptType = Enum.Parse<PromptType>(result.Type, true),
                CreateTime = result.CreateTime,
                CreateUserId = result.CreateUserId
            });
        }
        else
        {
            count = await query.CountAsync();

            var result = await query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Tags,
                    x.AvatarPath,
                    x.CreateTime,
                    x.UpdateTime,
                    x.CreateUserId,
                    x.Type,
                    x.TeamId,
                    TeamName = _databaseContext.Teams.Where(a => a.Id == x.TeamId).Select(a => a.Name).FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            promptItems.AddRange(result.Select(x => new PromptItem
            {
                Id = x.Id,
                TeamId = x.TeamId,
                TeamName = x.TeamName ?? string.Empty,
                Name = x.Name,
                Description = x.Description,
                Tags = x.Tags.Split(',').ToList(),
                AvatarPath = x.AvatarPath,
                PromptType = Enum.Parse<PromptType>(x.Type, true),
                CreateTime = x.CreateTime,
                CreateUserId = x.CreateUserId
            }).ToArray());
        }

        await _mediator.Send(new FillUserInfoCommand { Items = promptItems });

        // 补全团队头像路径
        var avatarUrls = await _mediator.Send(new QueryPublicFileUrlFromPathCommand { ObjectKeys = promptItems.Select(x => x.AvatarPath).ToArray() });
        foreach (var item in promptItems)
        {
            if (avatarUrls.Urls.TryGetValue(item.AvatarPath, out var url))
            {
                item.AvatarPath = url;
            }
            else
            {
                item.AvatarPath = new Uri(new Uri(_systemOptions.Server), "default/avatar.png").ToString();
            }
        }

        return new QueryPromptListCommandResponse
        {
            Count = count,
            Items = promptItems,
        };
    }
}
