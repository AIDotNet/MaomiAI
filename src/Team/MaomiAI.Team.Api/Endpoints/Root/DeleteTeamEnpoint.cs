// <copyright file="DeleteTeamEnpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Team.Shared.Commands.Root;
using MediatR;

namespace MaomiAI.Team.Api.Endpoints.Root;

/// <summary>
/// 删除团队.
/// </summary>
[EndpointGroupName("team")]
[HttpDelete($"{TeamApi.ApiPrefix}/{{teamId}}/delete")]
public class DeleteTeamEnpoint : Endpoint<DeleteTeamCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteTeamEnpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public DeleteTeamEnpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override async Task<EmptyCommandResponse> ExecuteAsync(DeleteTeamCommand req, CancellationToken ct)
    {
        await _mediator.Send(req);
        return EmptyCommandResponse.Default;
    }
}