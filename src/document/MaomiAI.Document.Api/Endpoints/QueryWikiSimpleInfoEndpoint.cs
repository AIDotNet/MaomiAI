// <copyright file="QueryWikiSimpleInfoEndpoint.cs" company="MaomiAI">
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
/// 查询知识库的简单信息.
/// </summary>
[EndpointGroupName("wiki")]
[HttpPost($"{DocumentApi.ApiPrefix}/simple_info")]
public class QueryWikiSimpleInfoEndpoint : Endpoint<QueryWikiSimpleInfoCommand, QueryWikiSimpleInfoResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiSimpleInfoEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryWikiSimpleInfoEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<QueryWikiSimpleInfoResponse> ExecuteAsync(QueryWikiSimpleInfoCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
