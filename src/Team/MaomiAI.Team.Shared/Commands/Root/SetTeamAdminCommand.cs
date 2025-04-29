using MediatR;

namespace MaomiAI.Team.Shared.Commands.Root;

/// <summary>
/// 设置成员为管理员.
/// </summary>
public class SetTeamAdminCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    public Guid TeamId { get; set; }

    /// <summary>
    /// 用户ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 设置为管理员，或取消管理员.
    /// </summary>
    public bool IsAdmin { get; set; }
}
