// <copyright file="QueryWikiDocumentTaskListEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Shared.Queries.Documents;
using MaomiAI.Document.Shared.Queries.Documents.Responses;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints.Documents;

/// <summary>
/// 获取文档任务列表.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/document/tasks")]
public class QueryWikiDocumentTaskListEndpoint : Endpoint<QueryWikiDocumentTaskListCommand, IReadOnlyCollection<WikiDocumentTaskItem>>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiDocumentTaskListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryWikiDocumentTaskListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<IReadOnlyCollection<WikiDocumentTaskItem>> ExecuteAsync(QueryWikiDocumentTaskListCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
