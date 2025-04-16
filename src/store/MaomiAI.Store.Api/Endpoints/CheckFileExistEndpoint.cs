// <copyright file="S3Controller.cs" company="MaomiAI">
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
/// 检查文件是否存在.
/// </summary>
[EndpointGroupName("store")]
[FastEndpoints.HttpPost($"{StoreApi.ApiPrefix}/check_exist")]
public class CheckFileExistEndpoint:Endpoint<CheckFileExistCommand, CheckFileExistCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckFileExistEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public CheckFileExistEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<CheckFileExistCommandResponse> ExecuteAsync(CheckFileExistCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}