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
/// 完成头像上传.
/// </summary>
[EndpointGroupName("wiki")]
[FastEndpoints.HttpPost($"{DocumentApi.ApiPrefix}/{{teamId}}/{{wikiId}}/upload_avatar")]
public class UploadWikiAvatarEndpoint : Endpoint<UploadWikiAvatarCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadWikiAvatarEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public UploadWikiAvatarEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(UploadWikiAvatarCommand req, CancellationToken ct)
        => _mediator.Send(req, ct);
}
