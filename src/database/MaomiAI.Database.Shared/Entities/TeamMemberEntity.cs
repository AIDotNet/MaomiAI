﻿using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 团队成员.
/// </summary>
public partial class TeamMemberEntity : IFullAudited
{
    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; set; }

    /// <summary>
    /// 用户id.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 是否为管理员.
    /// </summary>
    public bool IsAdmin { get; set; }

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
    /// id.
    /// </summary>
    public Guid Id { get; set; }
}
