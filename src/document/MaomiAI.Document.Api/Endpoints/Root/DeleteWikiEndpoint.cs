// <copyright file="DeleteWikiEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Shared.Commands;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints.Root;

/// <summary>
/// 删除知识库.
/// </summary>
[EndpointGroupName("wiki")]
[HttpDelete($"{DocumentApi.ApiPrefix}/delete")]
public class DeleteWikiEndpoint : Endpoint<DeleteWikiCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteWikiEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public DeleteWikiEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<EmptyCommandResponse> ExecuteAsync(DeleteWikiCommand req, CancellationToken ct)
    {
        var isAdmin = await _mediator.Send(new QueryUserIsTeamAdminCommand { TeamId = req.TeamId, UserId = _userContext.UserId });

        if (!isAdmin.IsOwner)
        {
            throw new BusinessException("只有团队所有者可以删除知识库") { StatusCode = 403 };
        }

        await _mediator.Send(req, ct);
        return EmptyCommandResponse.Default;
    }
}
