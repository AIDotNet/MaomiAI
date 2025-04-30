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
/// 删除知识库.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpDelete($"{DocumentApi.ApiPrefix}/{{teamId}}/{{wikiId}}/delete")]
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
