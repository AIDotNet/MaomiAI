// <copyright file="QueryAiModelDefaultConfigurationsEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.AiModel.Shared.Queries;
using MaomiAI.AiModel.Shared.Queries.Respones;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.AiModel.Api.Endpoints;

/// <summary>
/// 获取供应商模型的默认配置.
/// </summary>
[EndpointGroupName("aimodel")]
[HttpPost($"{AiModelApi.ApiPrefix}/aimodel/defaultconfiguration")]
public class QueryAiModelDefaultConfigurationsEndpoint : Endpoint<QueryAiModelDefaultConfigurationsCommand, QueryAiModelDefaultConfigurationsResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryAiModelDefaultConfigurationsEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryAiModelDefaultConfigurationsEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override async Task<QueryAiModelDefaultConfigurationsResponse> ExecuteAsync(QueryAiModelDefaultConfigurationsCommand req, CancellationToken ct)
    {
        return await _mediator.Send(req);
    }
}