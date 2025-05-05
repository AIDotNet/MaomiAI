using FastEndpoints;
using MaomiAI.User.Shared.Commands;
using MediatR;

namespace MaomiAI.User.Api.Endpoints;

/// <summary>
/// 结束上传头像.
/// </summary>
[EndpointGroupName("user")]
[HttpPost($"{UserApi.ApiPrefix}/uploadavatar")]
public class UploadtUserAvatarEndpoint : Endpoint<UploadtUserAvatarCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadtUserAvatarEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public UploadtUserAvatarEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(UploadtUserAvatarCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}
