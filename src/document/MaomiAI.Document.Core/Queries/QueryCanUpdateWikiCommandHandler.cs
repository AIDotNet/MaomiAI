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

/// <summary>
/// 用户是否可以修改知识库.
/// </summary>
public class QueryCanUpdateWikiCommandHandler : IRequestHandler<QueryCanUpdateWikiCommand, bool>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryCanUpdateWikiCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="mediator"></param>
    public QueryCanUpdateWikiCommandHandler(DatabaseContext databaseContext, IMediator mediator)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<bool> Handle(QueryCanUpdateWikiCommand request, CancellationToken cancellationToken)
    {
        var teamId = await _databaseContext.TeamWikis.Where(x => x.Id == request.WikiId).Select(x => x.TeamId).FirstOrDefaultAsync(cancellationToken);

        if (teamId == default)
        {
            throw new BusinessException("知识库不存在") { StatusCode = 404 };
        }

        var isAdmin = await _mediator.Send(new QueryUserIsTeamAdminCommand { TeamId = teamId, UserId = request.UserId });
        return isAdmin.IsAdmin;
    }
}
