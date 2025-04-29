// <copyright file="QueryTeamAdminIdsHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Queries.Admin;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Team.Core.Queries;

/// <summary>
/// 获取成员列表.
/// </summary>
public class QueryTeamMemberListHandler : IRequestHandler<QueryTeamMemberListCommand, PagedResult<TeamMemberResponse>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamMemberListHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mediator"></param>
    public QueryTeamMemberListHandler(DatabaseContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<PagedResult<TeamMemberResponse>> Handle(QueryTeamMemberListCommand request, CancellationToken cancellationToken)
    {
        List<TeamMemberResponse> adminList = new List<TeamMemberResponse>();

        var query = _dbContext.Users
            .Join(
            _dbContext.TeamMembers.Where(x => x.TeamId == request.TeamId),
            u => u.Id,
            tm => tm.UserId,
            (u, tm) => new TeamMemberResponse
            {
                UserId = u.Id,
                UserName = u.UserName,
                NickName = u.NickName,
                UserAvatarPath = u.AvatarPath,
                IsAdmin = tm.IsAdmin,
                IsOwner = false,
                CreateUserId = tm.CreateUserId,
                CreateTime = tm.CreateTime,
                UpdateUserId = tm.UpdateUserId,
                UpdateTime = tm.UpdateTime
            });

        var totalCount = await query.CountAsync(cancellationToken);

        var admins = await query
            .Take(request.Take)
            .Skip(request.Skip)
            .ToArrayAsync(cancellationToken);

        if (admins.Length > 0)
        {
            adminList.AddRange(admins);
        }

        await _mediator.Send(new FillUserInfoCommand<TeamMemberResponse>
        {
            Items = adminList
        });

        return new PagedResult<TeamMemberResponse>
        {
            Items = adminList,
            PageNo = request.PageNo,
            PageSize = request.PageSize,
            Total = totalCount
        };
    }
}