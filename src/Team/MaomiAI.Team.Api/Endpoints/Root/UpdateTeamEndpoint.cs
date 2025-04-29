// <copyright file="CreateTeamEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Team.Shared.Commands.Root;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints.Root;

/// <summary>
/// 更新团队信息.
/// </summary>
[EndpointGroupName("team")]
[HttpPost($"{TeamApi.ApiPrefix}/{{id}}/update")]
[Authorize]
public class UpdateTeamEndpoint : Endpoint<UpdateTeamInfoCommand, EmptyDto>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTeamEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public UpdateTeamEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override async Task<EmptyDto> ExecuteAsync(UpdateTeamInfoCommand req, CancellationToken ct)
    {
        await _mediator.Send(req);
        return EmptyDto.Default;
    }
}