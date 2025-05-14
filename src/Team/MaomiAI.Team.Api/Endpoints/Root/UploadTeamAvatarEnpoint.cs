// <copyright file="UploadTeamAvatarendpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Team.Shared.Commands.Root;
using MediatR;

namespace MaomiAI.Team.Api.Endpoints.Root;

/// <summary>
/// 上传头像.
/// </summary>
[EndpointGroupName("team")]
[HttpPost($"{TeamApi.ApiPrefix}/config/uploadavatar")]
public class UploadTeamAvatarendpoint : Endpoint<UploadTeamAvatarCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    public UploadTeamAvatarendpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override Task<EmptyCommandResponse> ExecuteAsync(UploadTeamAvatarCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}
