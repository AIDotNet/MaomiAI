// <copyright file="QuerySupportModelProviderEndpoint.cs" company="MaomiAI">
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
/// 查询支持的供应商列表和配置.
/// </summary>
[EndpointGroupName("aimodel")]
[HttpGet("aimodel/support_provider")]
public class QuerySupportModelProviderEndpoint : Endpoint<EmptyRequest, QuerySupportModelProviderCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuerySupportModelProviderEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QuerySupportModelProviderEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override async Task<QuerySupportModelProviderCommandResponse> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return await _mediator.Send(new QuerySupportModelProviderCommand { });
    }
}