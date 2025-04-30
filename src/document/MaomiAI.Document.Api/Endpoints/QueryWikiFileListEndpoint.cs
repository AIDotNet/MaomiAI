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
/// 知识库文件列表.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/{{teamId}}/{{wikiId}}/documents")]
public class QueryWikiFileListEndpoint : Endpoint<QueryWikiFileListCommand, QueryWikiFileListResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiFileListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryWikiFileListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<QueryWikiFileListResponse> ExecuteAsync(QueryWikiFileListCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
