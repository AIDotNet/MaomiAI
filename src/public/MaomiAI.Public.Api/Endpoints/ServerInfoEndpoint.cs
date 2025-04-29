// <copyright file="ServerInfoEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Public.Queries;
using MaomiAI.Public.Queries.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Public.Endpoints;

/// <summary>
/// 获取服务器信息.
/// </summary>
[EndpointGroupName("public")]
[HttpGet($"{PublicApi.ApiPrefix}/serverinfo")]
[AllowAnonymous]
public class ServerInfoEndpoint : EndpointWithoutRequest<QueryServerInfoResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerInfoEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public ServerInfoEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<QueryServerInfoResponse> ExecuteAsync(CancellationToken ct)
    {
        return _mediator.Send(new QueryServerInfoCommand { }, ct);
    }
}
