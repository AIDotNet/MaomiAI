﻿// <copyright file="SetDefaultAiModelEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.AiModel.Shared.Commands;
using MaomiAI.Team.Shared.Queries;
using MediatR;

namespace MaomiAI.AiModel.Api.Endpoints;

/// <summary>
/// 设置某个领域下默认的 ai 模型.
/// </summary>
[EndpointGroupName("aimodel")]
[HttpPost($"{AiModelApi.ApiPrefix}/setdefault")]
public class SetDefaultAiModelEndpoint : Endpoint<SetDefaultAiModelCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetDefaultAiModelEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public SetDefaultAiModelEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<EmptyCommandResponse> ExecuteAsync(SetDefaultAiModelCommand req, CancellationToken ct)
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

        return await _mediator.Send(req);
    }
}
