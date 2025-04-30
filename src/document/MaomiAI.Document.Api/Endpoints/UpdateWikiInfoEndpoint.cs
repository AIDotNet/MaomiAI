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
/// 更新知识库信息.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/{{teamId}}/{{wikiId}}/update")]
public class UpdateWikiInfoEndpoint : Endpoint<UpdateWikiInfoCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateWikiInfoEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public UpdateWikiInfoEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(UpdateWikiInfoCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
