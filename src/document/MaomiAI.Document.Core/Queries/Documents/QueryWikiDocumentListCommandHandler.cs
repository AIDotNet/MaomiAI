// <copyright file="QueryWikiDocumentListCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Document.Shared.Models;
using MaomiAI.Document.Shared.Queries;
using MaomiAI.Document.Shared.Queries.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Queries.Documents;

/// <summary>
/// 查询知识库文档列表.
/// </summary>
public class QueryWikiDocumentListCommandHandler : IRequestHandler<QueryWikiDocumentListCommand, QueryWikiDocumentListResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiDocumentListCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="mediator"></param>
    public QueryWikiDocumentListCommandHandler(DatabaseContext databaseContext, IMediator mediator)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<QueryWikiDocumentListResponse> Handle(QueryWikiDocumentListCommand request, CancellationToken cancellationToken)
    {
        var query = _databaseContext.TeamWikiDocuments.Where(x => x.WikiId == request.WikiId);

        if (!string.IsNullOrEmpty(request.Query))
        {
            query = query.Where(x => x.FileName.Contains(request.Query));
        }

        var totalCount = await query.CountAsync();

        var result = await query.Join(_databaseContext.Files.Where(x => x.IsUpload), a => a.FileId, b => b.Id, (a, b) => new QueryWikiDocumentListItem
        {
            DocumentId = a.Id,
            FileName = b.FileName,
            FileSize = b.FileSize,
            ContentType = b.ContentType,
            CreateTime = a.CreateTime,
            CreateUserId = a.CreateUserId,
            UpdateTime = a.UpdateTime,
            UpdateUserId = a.UpdateUserId,
            Embedding = _databaseContext.TeamWikiDocumentTasks.Any(x => x.DocumentId == a.Id && x.State == (int)FileEmbeddingState.Successful)
        }).Take(request.Take).Skip(request.Skip).ToArrayAsync();

        await _mediator.Send(new FillUserInfoCommand
        {
            Items = result
        });

        return new QueryWikiDocumentListResponse
        {
            Total = totalCount,
            PageNo = request.PageNo,
            PageSize = request.PageSize,
            Items = result
        };
    }
}
