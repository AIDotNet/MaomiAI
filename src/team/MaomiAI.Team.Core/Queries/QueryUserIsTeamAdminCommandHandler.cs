// <copyright file="QueryTeamAdminIdsHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Team.Shared.Queries;
using MaomiAI.Team.Shared.Queries.Admin;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Team.Core.Queries;

/// <summary>
/// 获取管理员 id 列表.
/// </summary>
public class QueryUserIsTeamAdminCommandHandler : IRequestHandler<QueryUserIsTeamAdminCommand, QueryUserIsTeamAdminCommandResponse>
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryUserIsTeamAdminCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public QueryUserIsTeamAdminCommandHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<QueryUserIsTeamAdminCommandResponse> Handle(QueryUserIsTeamAdminCommand request, CancellationToken cancellationToken)
    {
        var wikiOwenerId = await _dbContext.Teams.Where(x => x.Id == request.TeamId && x.OwnerId == request.UserId)
            .Select(x => x.OwnerId)
            .FirstOrDefaultAsync();

        if (wikiOwenerId == default)
        {
            throw new BusinessException("团队不存在") { StatusCode = 404 };
        }

        if (wikiOwenerId == request.UserId)
        {
            return new QueryUserIsTeamAdminCommandResponse
            {
                IsOwner = true,
                IsAdmin = true
            };
        }

        var isAdmin = await _dbContext.TeamMembers
            .Where(x => x.TeamId == request.TeamId && x.UserId == request.UserId && x.IsAdmin == true)
            .AnyAsync(cancellationToken);

        return new QueryUserIsTeamAdminCommandResponse
        {
            IsOwner = false,
            IsAdmin = isAdmin
        };
    }
}
