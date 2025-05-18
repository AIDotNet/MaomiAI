// <copyright file="QueryTeamDetailEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Team.Shared.Queries;
using MaomiAI.Team.Shared.Queries.Admin;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;

namespace MaomiAI.Team.Api.Endpoints.User;

/// <summary>
/// 查询团队详细信息.
/// </summary>
[EndpointGroupName("team")]
[HttpGet($"{TeamApi.ApiPrefix}/teamdetail")]
public class QueryTeamDetailEndpoint : Endpoint<QueryTeamDetailCommand, QueryTeamDetailCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamDetailEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public QueryTeamDetailEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<QueryTeamDetailCommandResponse> ExecuteAsync(QueryTeamDetailCommand req, CancellationToken ct)
    {
        var isAdmin = await _mediator.Send(new QueryUserIsTeamMemberCommand
        {
            TeamId = req.TeamId,
            UserId = _userContext.UserId
        });

        if (!isAdmin.IsMember && !isAdmin.IsPublic)
        {
            throw new BusinessException("不是该团队成员.") { StatusCode = 403 };
        }

        return await _mediator.Send(req, ct);
    }
}
