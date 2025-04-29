// <copyright file="LoginEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.User.Shared.Commands;
using MediatR;

namespace MaomiAI.User.Api.Endpoints;

/// <summary>
/// 重置密码.
/// </summary>
[EndpointGroupName("user")]
[HttpPost($"{UserApi.ApiPrefix}/resetpassword")]
public class UpdateUserPasswordEndpoint : Endpoint<UpdateUserPasswordCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserPasswordEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public UpdateUserPasswordEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override async Task<EmptyCommandResponse> ExecuteAsync(UpdateUserPasswordCommand req, CancellationToken ct)
    {
        return await _mediator.Send(req, ct);
    }
}
