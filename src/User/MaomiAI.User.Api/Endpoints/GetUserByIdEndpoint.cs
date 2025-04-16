// <copyright file="GetUserByIdEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.User.Shared.Models;
using MaomiAI.User.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.User.Api.Endpoints;

/// <summary>
/// 根据ID获取用户.
/// </summary>
[EndpointGroupName("user")]
[HttpGet($"{UserApi.ApiPrefix}/get-user/{{id}}")]
[Authorize]
public class GetUserByIdEndpoint : EndpointWithoutRequest<UserDto>
{
    private readonly IMediator _mediator;

    public GetUserByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<UserDto> ExecuteAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var query = new GetUserByIdQuery { Id = id };
        var result = await _mediator.Send(query, ct);
        if (result == null)
        {
            await SendNotFoundAsync(ct);
            return null!;
        }

        return result;
    }
} 