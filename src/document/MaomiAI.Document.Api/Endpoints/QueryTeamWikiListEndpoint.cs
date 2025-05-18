// <copyright file="QueryTeamWikiListEndpoint.cs" company="MaomiAI">
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
/// 查询团队知识库列表.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpGet($"{DocumentApi.ApiPrefix}/wikis")]
public class QueryTeamWikiListEndpoint : Endpoint<QueryTeamWikiListCommand, IReadOnlyCollection<QueryWikiSimpleInfoResponse>>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamWikiListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryTeamWikiListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<IReadOnlyCollection<QueryWikiSimpleInfoResponse>> ExecuteAsync(QueryTeamWikiListCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}
