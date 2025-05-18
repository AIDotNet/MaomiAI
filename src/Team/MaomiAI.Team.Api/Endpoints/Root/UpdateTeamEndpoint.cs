// <copyright file="UpdateTeamEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Team.Shared.Commands.Root;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints.Root;

/// <summary>
/// 更新团队信息.
/// </summary>
[EndpointGroupName("team")]
[HttpPost($"{TeamApi.ApiPrefix}/info")]
[Authorize]
public class UpdateTeamEndpoint : Endpoint<UpdateTeamInfoCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTeamEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public UpdateTeamEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<EmptyCommandResponse> ExecuteAsync(UpdateTeamInfoCommand req, CancellationToken ct)
    {
        var isAdmin = await _mediator.Send(new QueryUserIsTeamAdminCommand
        {
            TeamId = req.TeamId,
            UserId = _userContext.UserId
        });

        if (!isAdmin.IsOwner)
        {
            throw new BusinessException("没有操作权限.") { StatusCode = 403 };
        }

        await _mediator.Send(req, ct);

        return EmptyCommandResponse.Default;
    }
}
