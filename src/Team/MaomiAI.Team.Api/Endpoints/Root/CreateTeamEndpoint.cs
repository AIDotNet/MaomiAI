// <copyright file="CreateTeamEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands.Root;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints.Root;

/// <summary>
/// 删除团队.
/// </summary>
[EndpointGroupName("team")]
[HttpPost($"{TeamApi.ApiPrefix}/create")]
[Authorize]
public class CreateTeamEndpoint : Endpoint<CreateTeamCommand, GuidResponse>
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
    public override Task<GuidResponse> ExecuteAsync(CreateTeamCommand req, CancellationToken ct)
        => _mediator.Send(req);
}