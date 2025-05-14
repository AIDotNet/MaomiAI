// <copyright file="PreUploadImageendpoint.cs" company="MaomiAI">
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
/// 获取预上传文件签名地址，只能用于上传公开类文件，如头像等.
/// </summary>
[EndpointGroupName("store")]
[FastEndpoints.HttpPost($"{StoreApi.ApiPrefix}/pre_upload_image")]
public class PreUploadImageendpoint : Endpoint<PreUploadImageCommand, PreUploadFileCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreUploadImageendpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public PreUploadImageendpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<PreUploadFileCommandResponse> ExecuteAsync(PreUploadImageCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
