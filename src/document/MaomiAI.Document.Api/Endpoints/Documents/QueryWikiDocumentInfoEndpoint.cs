// <copyright file="QueryWikiFileEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Shared.Queries.Documents;
using MaomiAI.Document.Shared.Queries.Response;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints.Documents;

/// <summary>
/// 获取知识库文档信息.
/// </summary>
[EndpointGroupName("wiki")]
[HttpPost($"{DocumentApi.ApiPrefix}/document/info")]
public class QueryWikiDocumentInfoEndpoint : Endpoint<QueryWikiDocumentInfoCommand, QueryWikiDocumentListItem>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiDocumentInfoEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryWikiDocumentInfoEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<QueryWikiDocumentListItem> ExecuteAsync(QueryWikiDocumentInfoCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
