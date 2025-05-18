// <copyright file="InviteUserToTeamEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Team.Shared.Commands.Admin;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints.Admin;

/// <summary>
/// 邀请用户加入团队.
/// </summary>
[EndpointGroupName("tram")]
[HttpPost($"{TeamApi.ApiPrefix}/member/invite")]
[Authorize]
public class InviteUserToTeamEndpoint : Endpoint<InviteUserToTeamCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="InviteUserToTeamEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public InviteUserToTeamEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<EmptyCommandResponse> ExecuteAsync(InviteUserToTeamCommand req, CancellationToken ct)
    {
        var isAdmin = await _mediator.Send(new QueryUserIsTeamAdminCommand
        {
            TeamId = req.TeamId,
            UserId = _userContext.UserId
        });

        if (!isAdmin.IsAdmin)
        {
            throw new BusinessException("没有操作权限.") { StatusCode = 403 };
        }

        return await _mediator.Send(req, ct);
    }
}
