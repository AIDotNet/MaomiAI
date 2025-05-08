// <copyright file="QueryTeamSimpleEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Team.Shared.Queries;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints.User;

/// <summary>
/// 查询团队详细信息.
/// </summary>
[EndpointGroupName("team")]
[HttpGet($"{TeamApi.ApiPrefix}/{{teamId}}/teamdetail")]
public class QueryTeamDetailEndpoint : Endpoint<QueryTeamDetailCommand, QueryTeamDetailCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamDetailEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryTeamDetailEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override Task<QueryTeamDetailCommandResponse> ExecuteAsync(QueryTeamDetailCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}
