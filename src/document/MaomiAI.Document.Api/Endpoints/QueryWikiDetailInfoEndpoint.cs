// <copyright file="QueryWikiDetailInfoEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Shared.Queries;
using MaomiAI.Document.Shared.Queries.Response;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints;

/// <summary>
/// 知识库详细信息.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/detail")]
public class QueryWikiDetailInfoEndpoint : Endpoint<QueryWikiDetailInfoCommand, QueryWikiDetailInfoResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiDetailInfoEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryWikiDetailInfoEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<QueryWikiDetailInfoResponse> ExecuteAsync(QueryWikiDetailInfoCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
