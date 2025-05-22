// <copyright file="PreUploadOpenApiFileEndpoint.cs" company="MaomiAI">
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
/// 插件列表.
/// </summary>
[EndpointGroupName("store")]
[FastEndpoints.HttpPost($"{PluginApi.ApiPrefix}/pluginlist")]
public class QueryPluginListEndpoint : Endpoint<QueryPluginListCommand, QueryPluginListCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryPluginListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryPluginListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override async Task<QueryPluginListCommandResponse> ExecuteAsync(QueryPluginListCommand req, CancellationToken ct)
    {
        return await _mediator.Send(req, ct);
    }
}
