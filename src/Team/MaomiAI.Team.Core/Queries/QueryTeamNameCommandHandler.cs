// <copyright file="CheckTeamNameCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Team.Core.Queries;

public class QueryTeamNameCommandHandler : IRequestHandler<QueryTeamNameCommand, ExistResponse>
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamNameCommandHandler"/> class.
    /// </summary>
    /// <param name="maomiaiContext"></param>
    public QueryTeamNameCommandHandler(DatabaseContext maomiaiContext)
    {
        _dbContext = maomiaiContext;
    }

    /// <inheritdoc/>
    public async Task<ExistResponse> Handle(QueryTeamNameCommand request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Teams.Where(x => x.Name == request.Name);
        if (request.Id != null)
        {
            query = query.Where(x => x.Id != request.Id);
        }

        var existRecord = await query.AnyAsync();

        return new ExistResponse
        {
            IsExist = existRecord
        };
    }
}
