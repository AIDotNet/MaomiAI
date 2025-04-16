// <copyright file="ToggleUserStatusEndpoint.cs" company="MaomiAI">
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
/// 启用或禁用用户.
/// </summary>
[EndpointGroupName("user")]
[HttpPost($"{UserApi.ApiPrefix}/toggle-status")]
[Authorize]
public class ToggleUserStatusEndpoint : Endpoint<ToggleUserStatusCommand, EmptyDto>
{
    private readonly IMediator _mediator;

    public ToggleUserStatusEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<EmptyDto> ExecuteAsync(ToggleUserStatusCommand req, CancellationToken ct)
    {
        await _mediator.Send(req, ct);
        await SendNoContentAsync(ct);
        return new EmptyDto();
    }
}