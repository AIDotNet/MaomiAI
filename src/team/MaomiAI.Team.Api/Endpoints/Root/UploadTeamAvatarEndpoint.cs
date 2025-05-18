// <copyright file="UploadTeamAvatarEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Team.Shared.Commands.Root;
using MaomiAI.Team.Shared.Queries;
using MediatR;

namespace MaomiAI.Team.Api.Endpoints.Root;

/// <summary>
/// 上传头像.
/// </summary>
[EndpointGroupName("team")]
[HttpPost($"{TeamApi.ApiPrefix}/config/uploadavatar")]
public class UploadTeamAvatarEndpoint : Endpoint<UploadTeamAvatarCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadTeamAvatarEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public UploadTeamAvatarEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<EmptyCommandResponse> ExecuteAsync(UploadTeamAvatarCommand req, CancellationToken ct)
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
