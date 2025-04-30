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
/// 知识库详细信息.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/{{teamId}}/{{wikiId}}/detail")]
public class QueryWikiDetailInfoEndpoint : Endpoint<QueryWikiDetailInfoCommand, QueryWikiDetailInfoResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiDetailInfoEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryWikiDetailInfoEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<QueryWikiDetailInfoResponse> ExecuteAsync(QueryWikiDetailInfoCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
