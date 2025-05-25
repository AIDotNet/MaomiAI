// <copyright file="QueryPrompListtEndpoints.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Prompt.Api;
using MaomiAI.Prompt.Commands;
using MaomiAI.Prompt.Queries;
using MaomiAI.Prompt.Queries.Responses;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.AiModel.Api.Endpoints;

/// <summary>
/// 查询提示词.
/// </summary>
[EndpointGroupName("prompt")]
[HttpPost("/prompt/list")]
public class QueryPrompListtEndpoints : Endpoint<QueryPromptListCommand, QueryPromptListCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryPrompListtEndpoints"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public QueryPrompListtEndpoints(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<QueryPromptListCommandResponse> ExecuteAsync(QueryPromptListCommand req, CancellationToken ct)
    {
        if (req.TeamId != null)
        {
            var isAdmin = await _mediator.Send(new QueryUserIsTeamMemberCommand
            {
                TeamId = req.TeamId.GetValueOrDefault(),
                UserId = _userContext.UserId
            });

            if (!isAdmin.IsMember)
            {
                throw new BusinessException("没有操作权限.") { StatusCode = 403 };
            }
        }

        return await _mediator.Send(req);
    }
}