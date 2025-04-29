using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 默认模型配置.
/// </summary>
public partial class TeamDefaultAiModelEntity : IFullAudited
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 模型id.
    /// </summary>
    public Guid ModelId { get; set; }

    /// <summary>
    /// 功能.
    /// </summary>
    public int Function { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset CreateTime { get; set; }

    public DateTimeOffset UpdateTime { get; set; }

    public Guid CreateUserId { get; set; }

    public Guid UpdateUserId { get; set; }

    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; set; }
}
