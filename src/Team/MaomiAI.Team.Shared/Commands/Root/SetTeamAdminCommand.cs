using MediatR;
using System.ComponentModel.DataAnnotations;

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
    /// 被邀请的用户ID.
    /// </summary>
    [Required]
    public Guid? UserId { get; init; }

    /// <summary>
    /// 用户名.
    /// </summary>
    public string? UserName { get; init; } = default!;

    /// <summary>
    /// 设置为管理员，或取消管理员.
    /// </summary>
    public bool IsAdmin { get; set; }
}
