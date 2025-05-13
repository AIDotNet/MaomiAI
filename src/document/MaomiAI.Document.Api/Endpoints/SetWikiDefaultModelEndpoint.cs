//using FastEndpoints;
//using MaomiAI.Document.Shared.Commands;
//using MediatR;
//using Microsoft.AspNetCore.Routing;

//namespace MaomiAI.Document.Api.Endpoints;

///// <summary>
///// 设置知识库默认的模型.
///// </summary>
//[EndpointGroupName("wiki")]
//[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/{{teamId}}/{{wikiId}}/settings/default_model")]
//public class SetWikiDefaultModelEndpoint : Endpoint<SetWikiDefaultModelCommand, EmptyCommandResponse>
//{
//    private readonly IMediator _mediator;

//    /// <summary>
//    /// Initializes a new instance of the <see cref="SetWikiDefaultModelEndpoint"/> class.
//    /// </summary>
//    /// <param name="mediator"></param>
//    public SetWikiDefaultModelEndpoint(IMediator mediator)
//    {
//        _mediator = mediator;
//    }

//    /// <inheritdoc/>
//    public override Task<EmptyCommandResponse> ExecuteAsync(SetWikiDefaultModelCommand req, CancellationToken ct)
//        => _mediator.Send(req, ct);
//}
