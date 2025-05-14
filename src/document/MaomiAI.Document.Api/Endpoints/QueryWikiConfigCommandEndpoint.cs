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
/// 查询知识库的配置.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpGet($"{DocumentApi.ApiPrefix}/{{teamId}}/{{wikiId}}/config")]
public class QueryWikiConfigCommandEndpoint : Endpoint<QueryWikiConfigCommand, QueryWikiConfigCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiConfigCommandEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryWikiConfigCommandEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<QueryWikiConfigCommandResponse> ExecuteAsync(QueryWikiConfigCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
