// <copyright file="LoginEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.User.Shared.Commands;
using MaomiAI.User.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.User.Api.Endpoints;

/// <summary>
/// 用户登录.
/// </summary>
[EndpointGroupName("user")]
[HttpPost($"{UserApi.ApiPrefix}/login")]
[AllowAnonymous]
public class LoginEndpoint : Endpoint<LoginCommand, LoginResult>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public LoginEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<LoginResult> ExecuteAsync(LoginCommand req, CancellationToken ct)
    {
        return await _mediator.Send(req, ct);
    }
}