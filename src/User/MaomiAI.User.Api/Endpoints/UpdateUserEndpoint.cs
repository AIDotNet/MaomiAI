// <copyright file="UpdateUserEndpoint.cs" company="MaomiAI">
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
/// 更新用户信息.
/// </summary>
[EndpointGroupName("user")]
[HttpPut($"{UserApi.ApiPrefix}/update-user/{{id}}")]
[Authorize]
public class UpdateUserEndpoint : Endpoint<UpdateUserCommand, EmptyDto>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public UpdateUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<EmptyDto> ExecuteAsync(UpdateUserCommand req, CancellationToken ct)
    {
        if (Route<string>("id") != req.Id.ToString())
        {
            await SendErrorsAsync(400, ct);
            return new EmptyDto();
        }

        await _mediator.Send(req, ct);
        await SendNoContentAsync(ct);
        return new EmptyDto();
    }
}