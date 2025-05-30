﻿using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 提示词.
/// </summary>
public partial class PromptEntity : IFullAudited
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 名称.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 描述.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// 助手设定,markdown.
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// 标签，使用逗号&quot;,&quot;分割多个标签值.
    /// </summary>
    public string Tags { get; set; } = default!;

    /// <summary>
    /// 头像路径.
    /// </summary>
    public string AvatarPath { get; set; } = default!;

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

    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; set; }

    /// <summary>
    /// 是否公开.
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// 分类.
    /// </summary>
    public string Type { get; set; } = default!;
}
