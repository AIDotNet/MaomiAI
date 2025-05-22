// <copyright file="QueryPluginGroupListEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Plugin.Shared.Commands;
using MaomiAI.Plugin.Shared.Commands.Responses;
using MaomiAI.Plugin.Shared.Queries;
using MaomiAI.Plugin.Shared.Queries.Responses;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Plugin.Api.Endpoints;

/// <summary>
/// 分组列表.
/// </summary>
[EndpointGroupName("store")]
[FastEndpoints.HttpPost($"{PluginApi.ApiPrefix}/grouplist")]
public class QueryPluginGroupListEndpoint : Endpoint<QueryPluginGroupListCommand, QueryPluginGroupListCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryPluginGroupListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryPluginGroupListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override async Task<QueryPluginGroupListCommandResponse> ExecuteAsync(QueryPluginGroupListCommand req, CancellationToken ct)
    {
        return await _mediator.Send(req, ct);
    }
}
