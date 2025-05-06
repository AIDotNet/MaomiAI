// <copyright file="LoginEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.User.Shared.Commands;
using MaomiAI.User.Shared.Commands.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.User.Api.Endpoints;

/// <summary>
/// 刷新token.
/// </summary>
[EndpointGroupName("user")]
[HttpPost($"{UserApi.ApiPrefix}/refresh_token")]
[AllowAnonymous]
public class RefreshTokenEndpoint : Endpoint<RefreshTokenCommand, LoginCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public RefreshTokenEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>、
    public override async Task<LoginCommandResponse> ExecuteAsync(RefreshTokenCommand req, CancellationToken ct)
    {
        return await _mediator.Send(req, ct);
    }
}