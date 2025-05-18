// <copyright file="ComplateUploadWikiDocumentEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Document.Shared.Commands.Documents;
using MaomiAI.Store.Commands.Response;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints.Documents;

/// <summary>
/// 完成文档上传.
/// </summary>
[EndpointGroupName("wiki")]
[HttpPost($"{DocumentApi.ApiPrefix}/document/complate_upload")]
public class ComplateUploadWikiDocumentEndpoint : Endpoint<ComplateUploadWikiDocumentCommand, ComplateFileCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComplateUploadWikiDocumentEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public ComplateUploadWikiDocumentEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<ComplateFileCommandResponse> ExecuteAsync(ComplateUploadWikiDocumentCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
