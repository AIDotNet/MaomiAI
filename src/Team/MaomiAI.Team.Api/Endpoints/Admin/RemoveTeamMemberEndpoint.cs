using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands.Admin;
using MaomiAI.Team.Shared.Commands.Root;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints.Admin;

/// <summary>
/// 创建团队.
/// </summary>
[EndpointGroupName("team")]
[HttpPost($"{TeamApi.ApiPrefix}/member/remove")]
[Authorize]
public class RemoveTeamMemberEndpoint : Endpoint<RemoveTeamMemberCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveTeamMemberEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public RemoveTeamMemberEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(RemoveTeamMemberCommand req, CancellationToken ct)
        => _mediator.Send(req);
}