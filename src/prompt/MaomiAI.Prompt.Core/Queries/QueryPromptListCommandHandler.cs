// <copyright file="QueryPromptListCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Prompt.Models;
using MaomiAI.Prompt.Queries;
using MaomiAI.Prompt.Queries.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Prompt.Core.Queries;

public class QueryPromptListCommandHandler : IRequestHandler<QueryPromptListCommand, QueryPromptListCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryPromptListCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public QueryPromptListCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<QueryPromptListCommandResponse> Handle(QueryPromptListCommand request, CancellationToken cancellationToken)
    {
        var query = _databaseContext.Prompts.AsQueryable();

        if (request.TeamId != null)
        {
            query = query.Where(x => x.TeamId == request.TeamId);
        }
        else
        {
            query = query.Where(x => x.IsPublic);
        }

        var result = await query
            .Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Tags = x.Tags,
                AvatarPath = x.AvatarPath,
            })
            .ToListAsync(cancellationToken);

        return new QueryPromptListCommandResponse
        {
            Items = result.Select(x => new PromptItem
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Tags = x.Tags.Split(',').ToList(),
                AvatarPath = x.AvatarPath,
            }).ToArray(),
        };
    }
}
