using System;
using System.Collections.Generic;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 团队成员
/// </summary>
public partial class TeamMember
{
    /// <summary>
    /// id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 团队id
    /// </summary>
    public int TeamId { get; set; }

    /// <summary>
    /// 用户id
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 是否管理员
    /// </summary>
    public bool IsAdmin { get; set; }
}
