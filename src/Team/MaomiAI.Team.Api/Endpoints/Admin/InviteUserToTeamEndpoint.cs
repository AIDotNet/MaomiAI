// <copyright file="InviteUserToTeamEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Team.Shared.Commands.Admin;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints.Admin;

/// <summary>
/// 邀请用户加入团队.
/// </summary>
[EndpointGroupName("tram")]
[HttpPost($"{TeamApi.ApiPrefix}/member/invite")]
[Authorize]
public class InviteUserToTeamEndpoint : Endpoint<InviteUserToTeamCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="InviteUserToTeamEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public InviteUserToTeamEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(InviteUserToTeamCommand req, CancellationToken ct)
        => _mediator.Send(req);
}
