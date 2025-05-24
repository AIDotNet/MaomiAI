// <copyright file="UploadTeamAvatarEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Prompt.Api;
using MaomiAI.Team.Shared.Commands.Root;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Team.Api.Endpoints.Root;

/// <summary>
/// 上传头像.
/// </summary>
[EndpointGroupName("team")]
[HttpPost($"{ApiPrompt.ApiPrefix}/avatar")]
public class UploadPromptAvatarEndpoint : Endpoint<UploadPromptAvatarCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadPromptAvatarEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public UploadPromptAvatarEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<EmptyCommandResponse> ExecuteAsync(UploadPromptAvatarCommand req, CancellationToken ct)
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
