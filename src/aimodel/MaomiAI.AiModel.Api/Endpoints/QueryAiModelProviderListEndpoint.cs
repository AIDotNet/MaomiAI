// <copyright file="QueryAiModelProviderListEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.AiModel.Shared.Queries;
using MaomiAI.AiModel.Shared.Queries.Respones;
using MaomiAI.Team.Shared.Queries;
using MediatR;

namespace MaomiAI.AiModel.Api.Endpoints;

/// <summary>
/// 查询团队的ai服务商列表.
/// </summary>
[EndpointGroupName("aimodel")]
[HttpGet($"{AiModelApi.ApiPrefix}/providerlist")]
public class QueryAiModelProviderListEndpoint : Endpoint<QueryAiModelProviderListCommand, QueryAiModelProviderListResponse>
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryAiModelProviderListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public QueryAiModelProviderListEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<QueryAiModelProviderListResponse> ExecuteAsync(QueryAiModelProviderListCommand req, CancellationToken ct)
    {
        var isAdmin = await _mediator.Send(new QueryUserIsTeamMemberCommand
        {
            TeamId = req.TeamId,
            UserId = _userContext.UserId
        });

        if (!isAdmin.IsMember)
        {
            throw new BusinessException("没有操作权限.") { StatusCode = 403 };
        }

        return await _mediator.Send(req);
    }
}
