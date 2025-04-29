// <copyright file="QueryTeamAdminListHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Team.Shared.Queries.Admin;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Team.Core.Queries;

/// <summary>
/// 获取管理员列表.
/// </summary>
public class QueryTeamAdminListHandler : IRequestHandler<QueryTeamAdminListCommand, ICollection<TeamMemberResponse>>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamAdminListHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mediator"></param>
    public QueryTeamAdminListHandler(DatabaseContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<ICollection<TeamMemberResponse>> Handle(QueryTeamAdminListCommand request, CancellationToken cancellationToken)
    {
        List<TeamMemberResponse> adminList = new List<TeamMemberResponse>();

        var admins = await _dbContext.Users
            .Where(x =>
            _dbContext.TeamMembers.Where(tm => tm.TeamId == request.TeamId && tm.IsAdmin == true).Any(tm => tm.UserId == x.Id))
            .Select(x => new TeamMemberResponse
            {
                UserId = x.Id,
                UserName = x.UserName,
                NickName = x.NickName,
                UserAvatarPath = x.AvatarPath,
                IsAdmin = true,
                IsOwner = false,
                CreateUserId = x.CreateUserId,
                CreateTime = x.CreateTime,
                UpdateUserId = x.UpdateUserId,
                UpdateTime = x.UpdateTime
            }).ToArrayAsync(cancellationToken);

        var ownUser = await _dbContext.Users
            .Where(x => _dbContext.Teams.Where(x => x.Id == request.TeamId).Any(x => x.OwnerId == x.Id))
            .Select(x => new TeamMemberResponse
            {
                UserId = x.Id,
                UserName = x.UserName,
                NickName = x.NickName,
                UserAvatarPath = x.AvatarPath,
                IsAdmin = true,
                IsOwner = true,
                CreateUserId = x.CreateUserId,
                CreateTime = x.CreateTime,
                UpdateUserId = x.UpdateUserId,
                UpdateTime = x.UpdateTime
            }).FirstOrDefaultAsync(cancellationToken);

        if (admins.Length > 0)
        {
            adminList.AddRange(admins);
        }

        if (ownUser != null)
        {
            adminList.Add(ownUser);
        }

        if (adminList.Count > 0)
        {
            await _mediator.Send(new FillUserInfoCommand<TeamMemberResponse>
            {
                Items = adminList
            });
        }

        return adminList;
    }
}
