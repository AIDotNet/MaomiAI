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
    public string AiModelType { get; set; } = default!;

    /// <summary>
    /// 是否删除.
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
    /// 创建人ID.
    /// </summary>
    public Guid CreateUserId { get; set; }

    /// <summary>
    /// 更新人ID.
    /// </summary>
    public Guid UpdateUserId { get; set; }

    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; set; }
}
