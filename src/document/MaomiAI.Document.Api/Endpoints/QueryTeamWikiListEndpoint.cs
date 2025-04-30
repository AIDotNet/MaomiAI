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
/// 查询团队知识库列表.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/{{teamId}}/wikis")]
public class QueryTeamWikiListEndpoint : Endpoint<QueryTeamWikiListCommand, ICollection<QueryWikiSimpleInfoResponse>>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamWikiListEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public QueryTeamWikiListEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<ICollection<QueryWikiSimpleInfoResponse>> ExecuteAsync(QueryTeamWikiListCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
