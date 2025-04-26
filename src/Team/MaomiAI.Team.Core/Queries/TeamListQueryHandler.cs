// <copyright file="TeamsQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Models;
using MaomiAI.Team.Shared.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Queries;

/// <summary>
/// 处理获取团队列表查询.
/// </summary>
public class TeamListQueryHandler : IRequestHandler<QueryUserJoinedTeamPagedCommand, PagedResult<UserJoinedTeamItemResponse>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMediator _mediator;
    private readonly ILogger<TeamListQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TeamListQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    public TeamListQueryHandler(DatabaseContext dbContext, ILogger<TeamListQueryHandler> logger, IMediator mediator)
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
    public async Task<PagedResult<UserJoinedTeamItemResponse>> Handle(QueryUserJoinedTeamPagedCommand request, CancellationToken cancellationToken)
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
            .Select(x => new UserJoinedTeamItemResponse
            {
                Name = x.Name,
                Id = x.Id,
                Description = x.Description,
                AvatarFileId = x.AvatarFileId,
                IsDisable = x.IsDisable,
                IsPublic = x.IsPublic,
                CreateTime = x.CreateTime,
                UpdateTime = x.UpdateTime,
                CreateUserId = x.CreateUserId,
                UpdateUserId = x.UpdateUserId,
            })
            .ToListAsync(cancellationToken);

        var response = await _mediator.Send(new UserInfoQuery<UserJoinedTeamItemResponse>
        {
            Items = result
        });

        return new PagedResult<UserJoinedTeamItemResponse>
        {
            Total = totalCount,
            PageNo = request.PageNo,
            PageSize = request.PageSize,
            Items = response
        };
    }
}