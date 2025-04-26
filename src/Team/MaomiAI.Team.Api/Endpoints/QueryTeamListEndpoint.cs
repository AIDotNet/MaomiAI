// <copyright file="CreateTeamEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands;
using MaomiAI.Team.Shared.Models;
using MaomiAI.Team.Shared.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints;

/// <summary>
/// 获取团队列表.
/// </summary>
[EndpointGroupName("team")]
[FastEndpoints.HttpPost($"{TeamApi.ApiPrefix}/list")]
public class QueryTeamListEndpoint : Endpoint<QueryUserJoinedTeamPagedCommand, PagedResult<UserJoinedTeamItemResponse>>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryTeamListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<PagedResult<UserJoinedTeamItemResponse>> ExecuteAsync(QueryUserJoinedTeamPagedCommand req, CancellationToken ct)
    {
        return _mediator.Send(req);
    }
}