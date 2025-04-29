// <copyright file="QueryTeamNameEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Queries;
using MediatR;

namespace MaomiAI.Team.Api.Endpoints;

/// <summary>
/// 检查团队名称是否存在.
/// </summary>
[EndpointGroupName("team")]
[FastEndpoints.HttpPost($"{TeamApi.ApiPrefix}/check_name")]
public class QueryTeamNameEndpoint : Endpoint<QueryTeamNameCommand, ExistResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamNameEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryTeamNameEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<ExistResponse> ExecuteAsync(QueryTeamNameCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}