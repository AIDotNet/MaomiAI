// <copyright file="QueryUserIsTeamMemberCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Team.Shared.Queries;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Team.Core.Queries;

/// <summary>
/// 用户是否团队成员.
/// </summary>
public class QueryUserIsTeamMemberCommandHandler : IRequestHandler<QueryUserIsTeamMemberCommand, QueryUserIsTeamMemberCommandResponse>
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryUserIsTeamMemberCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public QueryUserIsTeamMemberCommandHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<QueryUserIsTeamMemberCommandResponse> Handle(QueryUserIsTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var wiki = await _dbContext.Teams.Where(x => x.Id == request.TeamId && x.OwnerId == request.UserId)
            .Select(x => new { x.OwnerId, x.IsPublic })
            .FirstOrDefaultAsync();

        if (wiki == default)
        {
            throw new BusinessException("团队不存在") { StatusCode = 404 };
        }

        if (wiki.OwnerId == request.UserId)
        {
            return new QueryUserIsTeamMemberCommandResponse
            {
                IsMember = true,
                IsOwner = true,
                IsAdmin = true,
                IsPublic = wiki.IsPublic,
            };
        }

        var member = await _dbContext.TeamMembers
            .Where(x => x.TeamId == request.TeamId && x.UserId == request.UserId)
            .Select(x => new { x.Id, x.IsAdmin })
            .FirstOrDefaultAsync(cancellationToken);

        if (member == null || member.Id == default)
        {
            return new QueryUserIsTeamMemberCommandResponse
            {
                IsMember = false,
                IsOwner = false,
                IsAdmin = false,
                IsPublic = wiki.IsPublic,
            };
        }

        return new QueryUserIsTeamMemberCommandResponse
        {
            IsMember = true,
            IsOwner = false,
            IsAdmin = member.IsAdmin,
            IsPublic = wiki.IsPublic,
        };
    }
}
