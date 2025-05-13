// <copyright file="QueryWikiFileListCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Document.Shared.Queries;
using MaomiAI.Document.Shared.Queries.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Queries;

public class QueryWikiFileListCommandHandler : IRequestHandler<QueryWikiFileListCommand, QueryWikiFileListResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiFileListCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="mediator"></param>
    public QueryWikiFileListCommandHandler(DatabaseContext databaseContext, IMediator mediator)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<QueryWikiFileListResponse> Handle(QueryWikiFileListCommand request, CancellationToken cancellationToken)
    {
        var query = _databaseContext.TeamWikiDocuments.Where(x => x.WikiId == request.WikiId);

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(x => x.FileName.Contains(request.Search));
        }

        var totalCount = await query.CountAsync();

        var result = await query.Join(_databaseContext.Files, a => a.FileId, b => b.Id, (a, b) => new QueryWikiFileListItem
        {
            DocumentId = a.Id,
            FileName = b.FileName,
            FileSize = b.FileSize,
            ContentType = b.ContentType,
            CreateTime = a.CreateTime,
            CreateUserId = a.CreateUserId,
            UpdateTime = a.UpdateTime,
            UpdateUserId = a.UpdateUserId
        }).Take(request.Take).Skip(request.Skip).ToArrayAsync();

        await _mediator.Send(new FillUserInfoCommand
        {
            Items = result
        });

        return new QueryWikiFileListResponse
        {
            Total = totalCount,
            PageNo = request.PageNo,
            PageSize = request.PageSize,
            Items = result
        };
    }
}
