// <copyright file="PreUploadEnpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Team.Shared.Commands;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Store.Controllers;

/// <summary>
/// 获取上传头像的地址.
/// </summary>
[EndpointGroupName("store")]
[FastEndpoints.HttpPost($"{StoreApi.ApiPrefix}/pre_upload_avatar")]
public class PreUploadAvatarEnpoint : Endpoint<PreUploadAvatarCommand, PreUploadFileCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreUploadAvatarEnpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public PreUploadAvatarEnpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<PreUploadFileCommandResponse> ExecuteAsync(PreUploadAvatarCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
