// <copyright file="QueryRepeatedUserNameEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.User.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.User.Api.Endpoints;

/// <summary>
/// 查询用户名是否重复.
/// </summary>
[EndpointGroupName("user")]
[HttpPost($"{UserApi.ApiPrefix}/checkname")]
[AllowAnonymous]
public class QueryRepeatedUserNameEndpoint : Endpoint<QueryRepeatedUserNameCommand, Simple<bool>>
{
    private readonly IMediator _mediator;
    public QueryRepeatedUserNameEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<Simple<bool>> ExecuteAsync(QueryRepeatedUserNameCommand req, CancellationToken ct)
    {
        var result = await _mediator.Send(req, ct);
        return result;
    }
}