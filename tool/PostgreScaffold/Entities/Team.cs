using System;
using System.Collections.Generic;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 团队
/// </summary>
public partial class Team
{
    /// <summary>
    /// id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 团队名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 团队描述
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 超级管理员id
    /// </summary>
    public int RootId { get; set; }

    /// <summary>
    /// 团队头像
    /// </summary>
    public string Avatar { get; set; } = null!;
}
