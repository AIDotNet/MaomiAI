using MaomiAI.Infra.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MaomiAI.Team.Shared.Commands.User;

/// <summary>
/// 退出团队.
/// </summary>
public class ExistTeamCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    [Required]
    public Guid TeamId { get; set; }
}
