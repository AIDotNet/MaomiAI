using FastEndpoints;
using MaomiAI.Document.Shared.Commands;
using MaomiAI.Store.Commands.Response;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints;

/// <summary>
/// 完成文档上传.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/{{teamId}}/{{wikiId}}/complate_upload")]
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
