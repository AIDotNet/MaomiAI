using MediatR;

namespace MaomiAI.Team.Shared.Queries;

/// <summary>
/// 该用户是否是团队管理员.
/// </summary>
public class QueryUserIsTeamAdminCommand : IRequest<ExistResponse>
{
    public Guid TeamId { get; init; }
    public Guid UserId { get; init; }
}
