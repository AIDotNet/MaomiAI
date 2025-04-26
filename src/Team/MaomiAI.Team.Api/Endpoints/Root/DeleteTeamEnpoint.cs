using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands.Root;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints.Root;

/// <summary>
/// 创建团队.
/// </summary>
[EndpointGroupName("team")]
[HttpPost($"{TeamApi.ApiPrefix}/delete")]
[Authorize]
public class DeleteTeamEnpoint : Endpoint<DeleteTeamCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteTeamEnpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public DeleteTeamEnpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override async Task<EmptyCommandResponse> ExecuteAsync(DeleteTeamCommand req, CancellationToken ct)
    {
        await _mediator.Send(req);
        return EmptyCommandResponse.Default;
    }
}