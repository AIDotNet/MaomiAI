// <copyright file="QueryWikiFileListEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Shared.Queries;
using MaomiAI.Document.Shared.Queries.Response;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints.Documents;

/// <summary>
/// 知识库文件列表.
/// </summary>
[EndpointGroupName("wiki")]
[HttpPost($"{DocumentApi.ApiPrefix}/document/list")]
public class QueryWikiDocumentListEndpoint : Endpoint<QueryWikiDocumentListCommand, QueryWikiDocumentListResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiDocumentListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryWikiDocumentListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<QueryWikiDocumentListResponse> ExecuteAsync(QueryWikiDocumentListCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
