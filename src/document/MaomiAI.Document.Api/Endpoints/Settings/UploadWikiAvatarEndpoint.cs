// <copyright file="UploadWikiAvatarEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Shared.Commands;
using MaomiAI.Document.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints.Settings;

/// <summary>
/// 完成头像上传.
/// </summary>
[EndpointGroupName("wiki")]
[HttpPost($"{DocumentApi.ApiPrefix}/settings/upload_avatar")]
public class UploadWikiAvatarEndpoint : Endpoint<UploadWikiAvatarCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadWikiAvatarEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public UploadWikiAvatarEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<EmptyCommandResponse> ExecuteAsync(UploadWikiAvatarCommand req, CancellationToken ct)
    {
        var isAdmin = await _mediator.Send(new QueryCanUpdateWikiCommand
        {
            WikiId = req.WikiId,
            UserId = _userContext.UserId
        });

        if (!isAdmin)
        {
            throw new BusinessException("没有操作权限.") { StatusCode = 403 };
        }

        return await _mediator.Send(req, ct);
    }
}