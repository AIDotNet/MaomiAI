// <copyright file="TeamListQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Team.Shared.Queries.Responses;
using MaomiAI.Team.Shared.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Queries;

/// <summary>
/// 处理获取团队列表查询.
/// </summary>
public class QueryUserJoinedTeamHandler : IRequestHandler<QueryUserJoinedTeamCommand, PagedResult<TeamSimpleResponse>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMediator _mediator;
    private readonly ILogger<QueryUserJoinedTeamHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryUserJoinedTeamHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    public QueryUserJoinedTeamHandler(DatabaseContext dbContext, ILogger<QueryUserJoinedTeamHandler> logger, IMediator mediator)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// 处理获取团队列表查询.
    /// </summary>
    /// <param name="request">查询请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>团队列表.</returns>
    public async Task<PagedResult<TeamSimpleResponse>> Handle(QueryUserJoinedTeamCommand request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Teams.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            query = query.Where(x => x.Name.Contains(request.Keyword));
        }

        query = query.OrderByDescending(x => x.CreateTime);

        var totalCount = await query.CountAsync(cancellationToken);

        var result = await query.OrderByDescending(x => x.CreateTime)
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new TeamSimpleResponse
            {
                Name = x.Name,
                Id = x.Id,
                Description = x.Description,
                AvatarId = x.AvatarId,
                AvatarPath = x.AvatarPath,
                IsDisable = x.IsDisable,
                IsPublic = x.IsPublic,
                CreateTime = x.CreateTime,
                UpdateTime = x.UpdateTime,
                CreateUserId = x.CreateUserId,
                UpdateUserId = x.UpdateUserId,
                OwnUserId = x.OwnerId
            })
            .ToListAsync(cancellationToken);

        _ = await _mediator.Send(new FillUserInfoCommand<TeamSimpleResponse>
        {
            Items = result
        });

        await _mediator.Send(new FillUserInfoFuncCommand<TeamSimpleResponse>
        {
            Items = result,
            GetUserId = x => x.Select(x => x.OwnUserId).ToArray(),
            SetUserName = (x, t) =>
            {
                if (x.TryGetValue(t.OwnUserId, out var name))
                {
                    t.OwnUserName = name;
                }
            }
        });

        return new PagedResult<TeamSimpleResponse>
        {
            Total = totalCount,
            PageNo = request.PageNo,
            PageSize = request.PageSize,
            Items = result
        };
    }
}