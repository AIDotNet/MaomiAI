using MediatR;

namespace MaomiAI.Team.Shared.Queries;

/// <summary>
/// 该用户是否是团队管理员.
/// </summary>
public class QueryUserIsTeamAdminCommand : IRequest<QueryUserIsTeamAdminCommandResponse>
{
    public Guid TeamId { get; init; }
    public Guid UserId { get; init; }
}

public class QueryUserIsTeamAdminCommandResponse
{
    public bool IsAdmin { get; set; }
    public bool IsOwner { get; init; }
}

/// <summary>
/// 查询用户是否团队成员.
/// </summary>
public class QueryUserIsTeamMemberCommand : IRequest<QueryUserIsTeamMemberCommandResponse>
{
    public Guid TeamId { get; init; }
    public Guid UserId { get; init; }
}

public class QueryUserIsTeamMemberCommandResponse
{
    public bool IsMember { get; init; }
    public bool IsOwner { get; init; }
    public bool IsAdmin { get; init; }
}