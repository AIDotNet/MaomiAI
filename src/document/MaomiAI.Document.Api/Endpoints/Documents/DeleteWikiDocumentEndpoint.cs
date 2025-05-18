// <copyright file="DeleteWikiDocumentEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Shared.Commands.Documents;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints.Documents;

/// <summary>
/// 删除文档.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpDelete($"{DocumentApi.ApiPrefix}/document/delete")]
public class DeleteWikiDocumentEndpoint : Endpoint<DeleteWikiDocumentCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteWikiDocumentEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public DeleteWikiDocumentEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(DeleteWikiDocumentCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
