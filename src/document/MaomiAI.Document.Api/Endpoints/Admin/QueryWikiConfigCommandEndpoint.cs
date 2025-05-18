// <copyright file="QueryWikiConfigCommandEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Shared.Commands;
using MaomiAI.Document.Shared.Commands.Responses;
using MaomiAI.Document.Shared.Queries;
using MaomiAI.Document.Shared.Queries.Response;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints.Admin;

/// <summary>
/// 查询知识库的配置.
/// </summary>
[EndpointGroupName("wiki")]
[HttpGet($"{DocumentApi.ApiPrefix}/config")]
public class QueryWikiConfigCommandEndpoint : Endpoint<QueryWikiConfigCommand, QueryWikiConfigCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiConfigCommandEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryWikiConfigCommandEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<QueryWikiConfigCommandResponse> ExecuteAsync(QueryWikiConfigCommand req, CancellationToken ct)
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
