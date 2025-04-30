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
/// 创建知识库.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/{{teamId}}/create")]
public class CreateWikiEndpoint : Endpoint<CreateWikiCommand, IdResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateWikiEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public CreateWikiEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<IdResponse> ExecuteAsync(CreateWikiCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
