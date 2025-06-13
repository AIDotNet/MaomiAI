using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 用户助手设置.
/// </summary>
public partial class UserAssistantConfigEntity : IFullAudited
{
    /// <summary>
    /// 用户id.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 头像.
    /// </summary>
    public string Icon { get; set; } = default!;

    /// <summary>
    /// 个人助手名字.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 助手描述.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// 软删除.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public DateTimeOffset CreateTime { get; set; }

    /// <summary>
    /// 更新时间.
    /// </summary>
    public DateTimeOffset UpdateTime { get; set; }

    /// <summary>
    /// 创建人.
    /// </summary>
    public Guid CreateUserId { get; set; }

    /// <summary>
    /// 更新人.
    /// </summary>
    public Guid UpdateUserId { get; set; }
}
