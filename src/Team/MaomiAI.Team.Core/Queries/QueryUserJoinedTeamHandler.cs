// <copyright file="QueryUserJoinedTeamHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Infra;
using MaomiAI.Store.Queries;
using MaomiAI.Team.Shared.Queries.Responses;
using MaomiAI.Team.Shared.Queries.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Queries;

/// <summary>
/// 处理获取团队列表查询.
/// </summary>
public class QueryUserJoinedTeamHandler : IRequestHandler<QueryUserJoinedTeamCommand, PagedResult<QueryTeamSimpleCommandResponse>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMediator _mediator;
    private readonly ILogger<QueryUserJoinedTeamHandler> _logger;
    private readonly UserContext _userContext;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryUserJoinedTeamHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    /// <param name="systemOptions"></param>
    public QueryUserJoinedTeamHandler(DatabaseContext dbContext, ILogger<QueryUserJoinedTeamHandler> logger, IMediator mediator, UserContext userContext, SystemOptions systemOptions)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mediator = mediator;
        _userContext = userContext;
        _systemOptions = systemOptions;
    }

    /// <summary>
    /// 处理获取团队列表查询.
    /// </summary>
    /// <param name="request">查询请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>团队列表.</returns>
    public async Task<PagedResult<QueryTeamSimpleCommandResponse>> Handle(QueryUserJoinedTeamCommand request, CancellationToken cancellationToken)
    {
        List<QueryTeamSimpleCommandResponse> result = new();
        int totalCount = 0;

        var query = _dbContext.Teams.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            query = query.Where(x => x.Name.Contains(request.Keyword));
        }

        if (request.IsRoot == true)
        {
            query = query.Where(x => x.OwnerId == _userContext.UserId);
        }

        if (request.IsRoot == false)
        {
            query = query.Where(x => x.OwnerId != _userContext.UserId);
        }

        query = query.OrderByDescending(x => x.CreateTime);

        if (request.IsAdmin == true)
        {
            totalCount = await query.Where(x => _dbContext.TeamMembers.Any(y => y.UserId == _userContext.UserId && y.IsAdmin)).CountAsync();

            result = await query
                .Join(_dbContext.TeamMembers.Where(y => y.UserId == _userContext.UserId && y.IsAdmin), x => x.Id, y => y.TeamId, (x, y) => new QueryTeamSimpleCommandResponse
                {
                    Name = x.Name,
                    Id = x.Id,
                    IsRoot = x.OwnerId == _userContext.UserId,
                    IsAdmin = y.IsAdmin,
                    Description = x.Description,
                    AvatarUrl = x.AvatarPath,
                    IsDisable = x.IsDisable,
                    IsPublic = x.IsPublic,
                    CreateTime = x.CreateTime,
                    UpdateTime = x.UpdateTime,
                    CreateUserId = x.CreateUserId,
                    UpdateUserId = x.UpdateUserId,
                    OwnUserId = x.OwnerId
                })
                .OrderByDescending(x => x.CreateTime)
                        .Skip((request.PageNo - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync(cancellationToken);
        }
        else
        {
            if (request.IsAdmin == false)
            {
                query = query.Where(x => _dbContext.TeamMembers.Any(y => y.UserId != _userContext.UserId && y.IsAdmin));
            }

            totalCount = await query.CountAsync();
            result = await query
                .Select(x => new QueryTeamSimpleCommandResponse
                {
                    Name = x.Name,
                    Id = x.Id,
                    IsRoot = x.OwnerId == _userContext.UserId,
                    IsAdmin = _dbContext.TeamMembers.Any(y => y.UserId != _userContext.UserId && y.IsAdmin),
                    Description = x.Description,
                    AvatarUrl = x.AvatarPath,
                    IsDisable = x.IsDisable,
                    IsPublic = x.IsPublic,
                    CreateTime = x.CreateTime,
                    UpdateTime = x.UpdateTime,
                    CreateUserId = x.CreateUserId,
                    UpdateUserId = x.UpdateUserId,
                    OwnUserId = x.OwnerId
                })
                .OrderByDescending(x => x.CreateTime)
                        .Skip((request.PageNo - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToListAsync(cancellationToken);
        }

        var response = new PagedResult<QueryTeamSimpleCommandResponse>
        {
            Total = totalCount,
            PageNo = request.PageNo,
            PageSize = request.PageSize,
            Items = result
        };

        if (result.Count == 0)
        {
            return response;
        }

        // 填充用户信息
        _ = await _mediator.Send(new FillUserInfoCommand
        {
            Items = result
        });

        // 补全所有者信息
        await _mediator.Send(new FillUserInfoFuncCommand
        {
            Items = result,
            GetUserId = x => x.OfType<QueryTeamSimpleCommandResponse>().Select(x => x.OwnUserId).ToArray(),
            SetUserName = (x, t) =>
            {
                if (x.TryGetValue(((QueryTeamSimpleCommandResponse)t).OwnUserId, out var name))
                {
                    ((QueryTeamSimpleCommandResponse)t).OwnUserName = name;
                }
            }
        });

        // 补全团队头像路径
        var avatarUrls = await _mediator.Send(new QueryPublicFileUrlFromPathCommand { ObjectKeys = result.Select(x => x.AvatarUrl).ToArray() });
        foreach (var item in result)
        {
            if (avatarUrls.Urls.TryGetValue(item.AvatarUrl, out var url))
            {
                item.AvatarUrl = url;
            }
            else
            {
                item.AvatarUrl = new Uri(new Uri(_systemOptions.Server), "default/avatar.png").ToString();
            }
        }

        return response;
    }
}