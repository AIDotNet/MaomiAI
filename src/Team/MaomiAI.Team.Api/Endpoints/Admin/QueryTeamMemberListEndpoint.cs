// <copyright file="InviteUserToTeamEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Public.Queries;
using MaomiAI.Public.Queries.Response;
using MaomiAI.Team.Shared.Commands.Admin;
using MaomiAI.Team.Shared.Queries.Admin;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints.Admin;

/// <summary>
/// 团队成员列表.
/// </summary>
[EndpointGroupName("tram")]
[HttpPost($"{TeamApi.ApiPrefix}/{{teamId}}/memberlist")]
[Authorize]
public class QueryTeamMemberListEndpoint : Endpoint<QueryTeamMemberListCommand, PagedResult<TeamMemberResponse>>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamMemberListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryTeamMemberListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<PagedResult<TeamMemberResponse>> ExecuteAsync(QueryTeamMemberListCommand req, CancellationToken ct)
        => _mediator.Send(req);
}
