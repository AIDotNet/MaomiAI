﻿// <copyright file="S3Controller.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>


using FastEndpoints;
using MaomiAI.Store.Commands;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Store.Queries;
using MaomiAI.Store.Queries.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Store.Controllers;

/// <summary>
/// 完成文件上传.
/// </summary>
[EndpointGroupName("store")]
[FastEndpoints.HttpPost($"{StoreApi.ApiPrefix}/complate_url")]
public class ComplateUploadEndpoint : Endpoint<ComplateFileUploadCommand, ComplateFileCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComplateUploadEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public ComplateUploadEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<ComplateFileCommandResponse> ExecuteAsync(ComplateFileUploadCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
