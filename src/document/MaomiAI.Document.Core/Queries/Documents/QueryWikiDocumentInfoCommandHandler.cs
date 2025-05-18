// <copyright file="QueryWikiDocumentInfoCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Document.Shared.Queries.Documents;
using MaomiAI.Document.Shared.Queries.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Queries.Documents;

/// <summary>
/// 查询知识库文档文件.
/// </summary>
public class QueryWikiDocumentInfoCommandHandler : IRequestHandler<QueryWikiDocumentInfoCommand, QueryWikiDocumentListItem>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiDocumentInfoCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="mediator"></param>
    public QueryWikiDocumentInfoCommandHandler(DatabaseContext databaseContext, IMediator mediator)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<QueryWikiDocumentListItem> Handle(QueryWikiDocumentInfoCommand request, CancellationToken cancellationToken)
    {
        var query = _databaseContext.TeamWikiDocuments.Where(x => x.Id == request.DocumentId);

        var result = await query.Join(_databaseContext.Files, a => a.FileId, b => b.Id, (a, b) => new QueryWikiDocumentListItem
        {
            DocumentId = a.Id,
            FileName = b.FileName,
            FileSize = b.FileSize,
            ContentType = b.ContentType,
            CreateTime = a.CreateTime,
            CreateUserId = a.CreateUserId,
            UpdateTime = a.UpdateTime,
            UpdateUserId = a.UpdateUserId
        }).FirstOrDefaultAsync();

        if (result == null)
        {
            throw new BusinessException("文档不存在") { StatusCode = 404 };
        }

        await _mediator.Send(new FillUserInfoCommand
        {
            Items = new List<AuditsInfo> { result }
        });

        return result;
    }
}
