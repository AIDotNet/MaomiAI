// <copyright file="SearchWikiDocumentTextEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Core.Handlers.Responses;
using MaomiAI.Document.Shared.Commands;
using MaomiAI.Document.Shared.Queries.Documents;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints.Documents;

/// <summary>
/// 搜索文档文本.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/document/search")]
public class SearchWikiDocumentTextEndpoint : Endpoint<SearchWikiDocumentTextCommand, SearchWikiDocumentTextCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchWikiDocumentTextEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public SearchWikiDocumentTextEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<SearchWikiDocumentTextCommandResponse> ExecuteAsync(SearchWikiDocumentTextCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
