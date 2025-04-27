using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.User.Shared.Commands;
using MediatR;

namespace MaomiAI.User.Api.Endpoints;

public class UploadtUserAvatarEndpoint : Endpoint<UploadtUserAvatarCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    public UploadtUserAvatarEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override Task<EmptyCommandResponse> ExecuteAsync(UploadtUserAvatarCommand req, CancellationToken ct)
    {
        return _mediator.Send(req, ct);
    }
}
