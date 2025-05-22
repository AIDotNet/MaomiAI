// <copyright file="QueryPluginGroupListCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Plugin.Shared.Queries;
using MaomiAI.Plugin.Shared.Queries.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Plugin.Core.Queries;

public class QueryPluginGroupListCommandHandler : IRequestHandler<QueryPluginGroupListCommand, QueryPluginGroupListCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly IMediator _mediator;

    public QueryPluginGroupListCommandHandler(DatabaseContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<QueryPluginGroupListCommandResponse> Handle(QueryPluginGroupListCommand request, CancellationToken cancellationToken)
    {
        var query = _dbContext.TeamPluginGroups.Where(x => x.TeamId == request.TeamId);
        if (request.GroupId != null)
        {
            query = query.Where(x => x.Id == request.GroupId);
        }

        var groups = await query
            .Select(x => new QueryPluginGroupListItem
            {
                Id = x.Id,
                Name = x.Name,
                Server = x.Server,
                Header = x.Header,
                Type = x.Type,
                TeamId = x.TeamId,
                Description = x.Description,
                CreateTime = x.CreateTime,
                CreateUserId = x.CreateUserId,
                UpdateTime = x.UpdateTime,
                UpdateUserId = x.UpdateUserId,
            }).ToArrayAsync();

        await _mediator.Send(new FillUserInfoCommand { Items = groups });

        return new QueryPluginGroupListCommandResponse { Items = groups };
    }
}
