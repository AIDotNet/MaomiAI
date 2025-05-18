// <copyright file="UpdateWikiConfigCommandEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Shared.Commands;
using MaomiAI.Document.Shared.Commands.Responses;
using MaomiAI.Document.Shared.Queries.Response;
using MaomiAI.Store.Commands.Response;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints.Settings;

/// <summary>
/// 更新知识库的配置.
/// </summary>
[EndpointGroupName("wiki")]
[HttpPost($"{DocumentApi.ApiPrefix}/settings/config")]
public class UpdateWikiConfigCommandEndpoint : Endpoint<UpdateWikiConfigCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateWikiConfigCommandEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public UpdateWikiConfigCommandEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(UpdateWikiConfigCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}