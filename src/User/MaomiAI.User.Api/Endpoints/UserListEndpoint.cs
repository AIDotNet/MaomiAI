// <copyright file="UsersController.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Team.Shared.Commands;
using MaomiAI.User.Shared.Models;
using MaomiAI.User.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;


namespace MaomiAI.User.Api.Endpoints;

/// <summary>
/// 用户列表.
/// </summary>
[EndpointGroupName("user")]
[FastEndpoints.HttpPost($"{UserApi.ApiPrefix}/user_list")]
[Authorize]
public class UserListEndpoint : Endpoint<GetUsersQuery, PagedResult<UserDto>>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public UserListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override Task<PagedResult<UserDto>> ExecuteAsync(GetUsersQuery req, CancellationToken ct) =>
        _mediator.Send(req, ct);
}