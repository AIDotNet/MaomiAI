using System;
using System.Collections.Generic;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 系统配置
/// </summary>
public partial class Setting
{
    /// <summary>
    /// id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 配置名称
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 配置项值
    /// </summary>
    public string Value { get; set; } = null!;
}
