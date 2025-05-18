using FastEndpoints;
using MaomiAI.Team.Api.Endpoints.Admin;
using MaomiAI.Team.Shared.Commands.Admin;
using MaomiAI.Team.Shared.Commands.Root;
using MaomiAI.Team.Shared.Queries;
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
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetTeamAdminEndpoint"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="userContext"></param>
    public SetTeamAdminEndpoint(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public override async Task<EmptyCommandResponse> ExecuteAsync(SetTeamAdminCommand req, CancellationToken ct)
    {
        var isAdmin = await _mediator.Send(new QueryUserIsTeamAdminCommand
        {
            TeamId = req.TeamId,
            UserId = _userContext.UserId
        });

        if (!isAdmin.IsOwner)
        {
            throw new BusinessException("没有操作权限.") { StatusCode = 403 };
        }

        return await _mediator.Send(req, ct);
    }
}
