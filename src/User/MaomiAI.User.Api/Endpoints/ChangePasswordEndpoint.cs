// <copyright file="ChangePasswordEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.User.Shared.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.User.Api.Endpoints;

/// <summary>
/// 修改密码.
/// </summary>
[EndpointGroupName("user")]
[HttpPost($"{UserApi.ApiPrefix}/change-password")]
[Authorize]
public class ChangePasswordEndpoint : Endpoint<ChangePasswordCommand, EmptyDto>
{
    private readonly IMediator _mediator;

    public ChangePasswordEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<EmptyDto> ExecuteAsync(ChangePasswordCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendNoContentAsync(ct);
        return new EmptyDto();
    }
}