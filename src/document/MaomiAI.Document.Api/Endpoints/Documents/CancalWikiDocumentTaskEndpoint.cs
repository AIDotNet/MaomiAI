// <copyright file="CancalWikiDocumentTaskEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Core.Handlers;
using MaomiAI.Document.Core.Handlers.Responses;
using MaomiAI.Document.Shared.Commands;
using MaomiAI.Document.Shared.Commands.Documents;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints.Documents;

/// <summary>
/// 取消任务.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/document/canal_tasks")]
public class CancalWikiDocumentTaskEndpoint : Endpoint<CancalWikiDocumentTaskCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancalWikiDocumentTaskEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public CancalWikiDocumentTaskEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(CancalWikiDocumentTaskCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
