// <copyright file="InviteUserToTeamEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Team.Shared.Queries.Admin;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints.Admin;

/// <summary>
/// 查询团队管理员列表.
/// </summary>
[EndpointGroupName("tram")]
[HttpGet($"{TeamApi.ApiPrefix}/{{teamId}}/adminlist")]
public class QueryTeamAdminListEndpoint : Endpoint<QueryTeamAdminListCommand, ICollection<TeamMemberResponse>>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamAdminListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryTeamAdminListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<ICollection<TeamMemberResponse>> ExecuteAsync(QueryTeamAdminListCommand req, CancellationToken ct)
        => _mediator.Send(req);
}
