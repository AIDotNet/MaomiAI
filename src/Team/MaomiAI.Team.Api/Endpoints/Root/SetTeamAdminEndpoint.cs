using FastEndpoints;
using MaomiAI.Team.Shared.Commands.Root;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Team.Api.Endpoints.Root;

/// <summary>
/// 设置团队管理员.
/// </summary>
[EndpointGroupName("team")]
[HttpPost($"{TeamApi.ApiPrefix}/setadmin")]
[Authorize]
public class SetTeamAdminEndpoint : Endpoint<SetTeamAdminCommand, EmptyCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetTeamAdminEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public SetTeamAdminEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public override Task<EmptyCommandResponse> ExecuteAsync(SetTeamAdminCommand req, CancellationToken ct)
        => _mediator.Send(req);
}
