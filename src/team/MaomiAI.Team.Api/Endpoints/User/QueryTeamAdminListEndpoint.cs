// <copyright file="QueryTeamAdminListEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Queries;
using MaomiAI.Team.Shared.Queries.Admin;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;

namespace MaomiAI.Team.Api.Endpoints.User;

/// <summary>
/// 查询团队管理员列表.
/// </summary>
[EndpointGroupName("tram")]
[HttpGet($"{TeamApi.ApiPrefix}/adminlist")]
public class QueryTeamAdminListEndpoint : Endpoint<QueryTeamAdminListCommand, ICollection<TeamMemberResponse>>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamAdminListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public QueryTeamAdminListEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<ICollection<TeamMemberResponse>> ExecuteAsync(QueryTeamAdminListCommand req, CancellationToken ct)
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
