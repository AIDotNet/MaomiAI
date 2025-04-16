// <copyright file="CheckTeamOwnerQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Team.Core.Queries;

/// <summary>
/// 获取管理员 id 列表.
/// </summary>
public class QueryTeamAdminIdsHandler : IRequestHandler<QueryTeamAdminIdsListReuqest, TeamAdminListIdsResponse>
{
    private readonly MaomiaiContext _dbContext;

    public QueryTeamAdminIdsHandler(MaomiaiContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TeamAdminListIdsResponse> Handle(QueryTeamAdminIdsListReuqest request, CancellationToken cancellationToken)
    {
        var ownId = await _dbContext.Teams.Where(x => x.Id == request.TeamId)
            .Select(x => x.OwnerId).FirstOrDefaultAsync();

        var adminIds = await _dbContext.TeamMembers
            .Where(x => x.TeamId == request.TeamId && x.IsAdmin == true)
            .Select(x => x.UserId)
            .ToListAsync(cancellationToken);

        if (ownId != default)
        {
            adminIds.Add(ownId);
        }

        return new TeamAdminListIdsResponse
        {
            OwnId = ownId,
            AdminIds = adminIds
        };
    }
}