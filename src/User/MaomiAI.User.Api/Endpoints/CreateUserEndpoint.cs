// <copyright file="CreateUserEndpoint.cs" company="MaomiAI">
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
/// 创建用户.
/// </summary>
[EndpointGroupName("user")]
[HttpPost($"{UserApi.ApiPrefix}/create-user")]
[AllowAnonymous]
public class CreateUserEndpoint : Endpoint<CreateUserCommand, Guid>
{
    private readonly IMediator _mediator;

    public CreateUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<Guid> ExecuteAsync(CreateUserCommand req, CancellationToken ct)
    {
        var userId = await _mediator.Send(req, ct);
        await SendCreatedAtAsync<GetUserByIdEndpoint>(new { id = userId }, userId, cancellation: ct);
        return userId;
    }
}