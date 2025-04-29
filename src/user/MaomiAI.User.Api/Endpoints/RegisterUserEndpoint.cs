// <copyright file="RegisterUserEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.User.Shared.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.User.Api.Endpoints;

/// <summary>
/// 注册账号.
/// </summary>
[EndpointGroupName("user")]
[HttpPost($"{UserApi.ApiPrefix}/register")]
[AllowAnonymous]
public class RegisterUserEndpoint : Endpoint<RegisterUserCommand, GuidResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterUserEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public RegisterUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<GuidResponse> ExecuteAsync(RegisterUserCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}