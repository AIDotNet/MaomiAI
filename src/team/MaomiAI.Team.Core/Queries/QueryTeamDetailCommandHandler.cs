// <copyright file="QueryTeamDetailQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Infra;
using MaomiAI.Store.Queries;
using MaomiAI.Team.Shared.Queries;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Queries;

/// <summary>
/// 获取团队详细信息.
/// </summary>
public class QueryTeamDetailCommandHandler : IRequestHandler<QueryTeamDetailCommand, QueryTeamDetailCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;
    private readonly ILogger<QueryTeamDetailCommandHandler> _logger;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamDetailCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    /// <param name="systemOptions"></param>
    public QueryTeamDetailCommandHandler(DatabaseContext dbContext, ILogger<QueryTeamDetailCommandHandler> logger, IMediator mediator, UserContext userContext, SystemOptions systemOptions)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mediator = mediator;
        _userContext = userContext;
        _systemOptions = systemOptions;
    }

    /// <summary>
    /// 处理根据ID获取团队查询.
    /// </summary>
    /// <param name="request">查询请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>团队信息.</returns>
    public async Task<QueryTeamDetailCommandResponse> Handle(QueryTeamDetailCommand request, CancellationToken cancellationToken)
    {
        var team = await _dbContext.Teams.Where(t => t.Id == request.TeamId)
            .Select(x => new QueryTeamDetailCommandResponse
            {
                Id = x.Id,
                IsRoot = x.OwnerId == _userContext.UserId,
                IsAdmin = _dbContext.TeamMembers.Any(tm => tm.TeamId == x.Id && tm.UserId == _userContext.UserId && tm.IsAdmin),
                Name = x.Name,
                Description = x.Description,
                AvatarUrl = x.AvatarPath,
                UpdateUserId = x.UpdateUserId,
                IsDisable = x.IsDisable,
                CreateTime = x.CreateTime,
                UpdateTime = x.UpdateTime,
                CreateUserId = x.CreateUserId,
                IsPublic = x.IsPublic,
                OwnUserId = x.OwnerId,
                OwnUserName = string.Empty,
                Markdown = x.Markdown
            }).FirstOrDefaultAsync(cancellationToken);

        if (team == null)
        {
            throw new BusinessException("未找到团队");
        }

        if (!team.IsPublic && team.OwnUserId != _userContext.UserId)
        {
            var joinedTeam = await _dbContext.TeamMembers.AnyAsync(x => x.TeamId == request.TeamId && x.UserId == _userContext.UserId);
            if (!joinedTeam)
            {
                throw new BusinessException("没有权限访问该团队");
            }
        }

        var avatarUrl = string.Empty;
        if (!string.IsNullOrEmpty(team.AvatarUrl))
        {
            var fileUrls = await _mediator.Send(new QueryPublicFileUrlFromPathCommand { ObjectKeys = new List<string>() { team.AvatarUrl } });
            avatarUrl = fileUrls.Urls.First().Value!;
        }
        else
        {
            avatarUrl = new Uri(new Uri(_systemOptions.Server), "default/avatar.png").ToString();
        }

        team.AvatarUrl = avatarUrl;

        _ = await _mediator.Send(new FillUserInfoCommand
        {
            Items = new List<QueryTeamDetailCommandResponse> { team }
        });

        return team;
    }
}
