using FastEndpoints;
using MaomiAI.Document.Shared.Commands;
using MaomiAI.Document.Shared.Commands.Responses;
using MaomiAI.Document.Shared.Queries;
using MaomiAI.Document.Shared.Queries.Response;
using MaomiAI.Store.Commands.Response;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints;

/// <summary>
/// 预上传知识库文档.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/{{teamId}}/{{wikiId}}/preupload")]
public class PreUploadWikiDocumentEndpoint : Endpoint<PreUploadWikiDocumentCommand, PreloadWikiDocumentResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreUploadWikiDocumentEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public PreUploadWikiDocumentEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<PreloadWikiDocumentResponse> ExecuteAsync(PreUploadWikiDocumentCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
