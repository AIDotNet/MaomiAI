// <copyright file="DeleteUserEndpoint.cs" company="MaomiAI">
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
/// 删除用户.
/// </summary>
[EndpointGroupName("user")]
[HttpDelete($"{UserApi.ApiPrefix}/delete-user/{{id}}")]
[Authorize]
public class DeleteUserEndpoint : EndpointWithoutRequest<EmptyDto>
{
    private readonly IMediator _mediator;

    public DeleteUserEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<EmptyDto> ExecuteAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var command = new DeleteUserCommand { Id = id };
        await _mediator.Send(command, ct);
        await SendNoContentAsync(ct);
        return new EmptyDto();
    }
}