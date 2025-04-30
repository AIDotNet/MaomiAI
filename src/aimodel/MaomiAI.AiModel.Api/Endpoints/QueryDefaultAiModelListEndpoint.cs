// <copyright file="QueryAiModelDefaultConfigurationsEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.AiModel.Shared.Queries;
using MaomiAI.AiModel.Shared.Queries.Respones;
using MediatR;

namespace MaomiAI.AiModel.Api.Endpoints;

/// <summary>
/// 获取供应商模型的默认配置.
/// </summary>
[EndpointGroupName("aimodel")]
[HttpPost($"{AiModelApi.ApiPrefix}/aimodel/defaultconfiguration")]
public class QueryDefaultAiModelListEndpoint : Endpoint<QueryDefaultAiModelListCommand, QueryDefaultAiModelListResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryDefaultAiModelListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryDefaultAiModelListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override async Task<QueryDefaultAiModelListResponse> ExecuteAsync(QueryDefaultAiModelListCommand req, CancellationToken ct)
    {
        return await _mediator.Send(req);
    }
}