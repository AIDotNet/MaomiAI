using FastEndpoints;
using MaomiAI.Document.Shared.Queries;
using MaomiAI.Document.Shared.Queries.Response;
using MediatR;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Document.Api.Endpoints;

/// <summary>
/// 获取知识库文件.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/{{teamId}}/{{wikiId}}/document")]
public class QueryWikiFileEndpoint : Endpoint<QueryWikiFileCommand, QueryWikiFileListItem>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiFileEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryWikiFileEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<QueryWikiFileListItem> ExecuteAsync(QueryWikiFileCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
