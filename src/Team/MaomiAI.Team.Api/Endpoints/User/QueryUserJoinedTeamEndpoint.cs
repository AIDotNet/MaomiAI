// <copyright file="QueryUserJoinedTeamEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Queries.Responses;
using MaomiAI.Team.Shared.Queries.User;
using MediatR;

namespace MaomiAI.Team.Api.Endpoints.User;

/// <summary>
/// 获取团队列表.
/// </summary>
[EndpointGroupName("team")]
[HttpPost($"{TeamApi.ApiPrefix}/joined_list")]
public class QueryUserJoinedTeamEndpoint : Endpoint<QueryUserJoinedTeamCommand, PagedResult<TeamSimpleResponse>>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryUserJoinedTeamEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryUserJoinedTeamEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<PagedResult<TeamSimpleResponse>> ExecuteAsync(QueryUserJoinedTeamCommand req, CancellationToken ct)
    {
        return _mediator.Send(req);
    }
}