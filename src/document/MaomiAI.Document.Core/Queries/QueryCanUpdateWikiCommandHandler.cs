// <copyright file="QueryCanUpdateWikiCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Document.Shared.Queries;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Queries;

public class QueryCanUpdateWikiCommandHandler : IRequestHandler<QueryCanUpdateWikiCommand, bool>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;

    public QueryCanUpdateWikiCommandHandler(DatabaseContext databaseContext, IMediator mediator)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
    }

    public async Task<bool> Handle(QueryCanUpdateWikiCommand request, CancellationToken cancellationToken)
    {
        var teamId = await _databaseContext.TeamWikis.Where(x => x.Id == request.WikiId).Select(x => x.TeamId).FirstOrDefaultAsync(cancellationToken);

        if (teamId == default)
        {
            return false;
        }

        var isAdmin = await _mediator.Send(new QueryUserIsTeamAdminCommand { TeamId = teamId, UserId = request.UserId });
        return isAdmin.IsExist;
    }
}
