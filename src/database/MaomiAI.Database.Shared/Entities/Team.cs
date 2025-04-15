using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 团队.
/// </summary>
public partial class TeamEntity : IFullAudited
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 团队描述.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 团队名称.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 团队头像.
    /// </summary>
    public string AvatarFileId { get; set; } = null!;

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
    /// 所有者 id.
    /// </summary>
    public Guid OwnId { get; set; }

    /// <summary>
    /// 创建人ID.
    /// </summary>
    public Guid CreateUserId { get; set; }

    /// <summary>
    /// 更新人ID.
    /// </summary>
    public Guid UpdateUserId { get; set; }

    /// <summary>
    /// 禁用团队.
    /// </summary>
    public bool IsDisable { get; set; }

    /// <summary>
    /// 团队详细介绍.
    /// </summary>
    public string Markdown { get; set; } = null!;

    /// <summary>
    /// 是否公开,能够被外部搜索.
    /// </summary>
    public bool IsPublic { get; set; }
}
