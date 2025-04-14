// <copyright file="PreUploadEnpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Team.Shared.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Store.Controllers;

/// <summary>
/// 获取可公开访问的文件上传预签名地址.
/// </summary>
[EndpointGroupName("store")]
[FastEndpoints.HttpPost($"{StoreApi.ApiPrefix}/pre_public_url")]
[Authorize]
public class PreUploadEnpoint : Endpoint<PublicPreUploadFileCommand, PreUploadFileCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreUploadEnpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public PreUploadEnpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<PreUploadFileCommandResponse> ExecuteAsync(PublicPreUploadFileCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}