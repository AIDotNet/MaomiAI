// <copyright file="QueryPluginListCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Plugin.Shared.Queries;
using MaomiAI.Plugin.Shared.Queries.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Plugin.Core.Queries;

public class QueryPluginListCommandHandler : IRequestHandler<QueryPluginListCommand, QueryPluginListCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    public QueryPluginListCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<QueryPluginListCommandResponse> Handle(QueryPluginListCommand request, CancellationToken cancellationToken)
    {
        var query = _databaseContext.TeamPlugins.Where(x => x.TeamId == request.TeamId).AsQueryable();
        if (request.GroupId != null)
        {
            query = query.Where(x => x.GroupId == request.GroupId);
        }

        if (request.PluginIds != null && request.PluginIds.Count > 0)
        {
            query = query.Where(x => request.PluginIds.Contains(x.Id));
        }

        var result = await query
                .Select(x => new QueryPluginListItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Path = x.Path,
                    Summary = x.Summary,
                    GroupId = x.GroupId,
                    GroupName = _databaseContext.TeamPluginGroups
                        .Where(g => g.Id == x.GroupId)
                        .Select(g => g.Name)
                        .FirstOrDefault() ?? string.Empty
                })
                .ToArrayAsync(cancellationToken);

        return new QueryPluginListCommandResponse
        {
            Items = result
        };
    }
}
