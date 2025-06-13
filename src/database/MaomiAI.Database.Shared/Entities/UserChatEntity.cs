using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 用户对话.
/// </summary>
public partial class UserChatEntity : IFullAudited
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 话题标题.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// 团队id，要标识用户在哪个团队上下文.
    /// </summary>
    public Guid TeamId { get; set; }

    public Guid ModelId { get; set; }
    public Guid WikiId { get; set; }

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

    public string ExecutionSettings { get; set; } = default!;

    public string PluginIds { get;set; } = default!;
}
