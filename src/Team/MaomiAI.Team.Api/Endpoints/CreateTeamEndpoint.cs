// <copyright file="CreateTeamEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints;

/// <summary>
/// 创建团队.
/// </summary>
[EndpointGroupName("team")]
[FastEndpoints.HttpPost($"{TeamApi.ApiPrefix}/create")]
[Authorize]
public class CreateTeamEndpoint : Endpoint<CreateTeamCommand, GuidDto>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTeamEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public CreateTeamEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<GuidDto> ExecuteAsync(CreateTeamCommand req, CancellationToken ct)
        => _mediator.Send(req);
}