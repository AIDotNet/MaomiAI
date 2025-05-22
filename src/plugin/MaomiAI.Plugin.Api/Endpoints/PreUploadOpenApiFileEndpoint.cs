// <copyright file="PreUploadOpenApiFileEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Plugin.Shared.Commands;
using MaomiAI.Plugin.Shared.Commands.Responses;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Plugin.Api.Endpoints;

/// <summary>
/// 预上传 openapi 文件.
/// </summary>
[EndpointGroupName("store")]
[FastEndpoints.HttpPost($"{PluginApi.ApiPrefix}/pre_upload_openapi")]
public class PreUploadOpenApiFileEndpoint : Endpoint<PreUploadOpenApiFileCommand, PreUploadOpenApiFileCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreUploadOpenApiFileEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public PreUploadOpenApiFileEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<PreUploadOpenApiFileCommandResponse> ExecuteAsync(PreUploadOpenApiFileCommand req, CancellationToken ct)
    {
        var isAdmin = await _mediator.Send(new QueryUserIsTeamAdminCommand
        {
            TeamId = req.TeamId,
            UserId = _userContext.UserId,
        });

        if (!isAdmin.IsAdmin)
        {
            throw new BusinessException("没有操作权限.") { StatusCode = 403 };
        }

        return await _mediator.Send(req, ct);
    }
}