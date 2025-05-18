// <copyright file="QueryTeamMemberListEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Team.Shared.Queries;
using MaomiAI.Team.Shared.Queries.Admin;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using static FastEndpoints.Ep;

namespace MaomiAI.Team.Api.Endpoints.User;

/// <summary>
/// 团队成员列表.
/// </summary>
[EndpointGroupName("tram")]
[HttpPost($"{TeamApi.ApiPrefix}/memberlist")]
[Authorize]
public class QueryTeamMemberListEndpoint : Endpoint<QueryTeamMemberListCommand, PagedResult<TeamMemberResponse>>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamMemberListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public QueryTeamMemberListEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<PagedResult<TeamMemberResponse>> ExecuteAsync(QueryTeamMemberListCommand req, CancellationToken ct)
    {
        var isAdmin = await _mediator.Send(new QueryUserIsTeamMemberCommand
        {
            TeamId = req.TeamId,
            UserId = _userContext.UserId
        });

        if (!isAdmin.IsMember)
        {
            throw new BusinessException("不是该团队成员.") { StatusCode = 403 };
        }

        return await _mediator.Send(req, ct);
    }
}