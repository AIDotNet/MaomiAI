// <copyright file="QueryUserInfoEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.User.Shared.Queries;
using MediatR;

namespace MaomiAI.User.Api.Endpoints;

/// <summary>
/// 查询用户基本信息.
/// </summary>
[EndpointGroupName("user")]
[HttpGet($"{UserApi.ApiPrefix}/info")]
public class QueryUserInfoEndpoint : Endpoint<EmptyRequest, QueryUserInfoCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryUserInfoEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public QueryUserInfoEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override Task<QueryUserInfoCommandResponse> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        var query = new QueryUserInfoCommand
        {
            UserId = _userContext.UserId
        };

        return _mediator.Send(query, ct);
    }
}
