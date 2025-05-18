// <copyright file="QueryTeamWikiListCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Document.Shared.Queries;
using MaomiAI.Document.Shared.Queries.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Queries;

/// <summary>
/// 查询团队知识库列表.
/// </summary>
public class QueryTeamWikiListCommandHandler : IRequestHandler<QueryTeamWikiListCommand, IReadOnlyCollection<QueryWikiSimpleInfoResponse>>
{
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamWikiListCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public QueryTeamWikiListCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<QueryWikiSimpleInfoResponse>> Handle(QueryTeamWikiListCommand request, CancellationToken cancellationToken)
    {
        var response = await _databaseContext.TeamWikis
            .Where(x => x.TeamId == request.TeamId)
            .Select(x => new QueryWikiSimpleInfoResponse
            {
                WikiId = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsPublic = x.IsPublic,
                AvatarUrl = x.AvatarPath
            })
            .ToListAsync(cancellationToken);

        return response;
    }
}
