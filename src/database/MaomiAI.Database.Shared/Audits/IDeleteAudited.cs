﻿using System.ComponentModel.DataAnnotations;

namespace MaomiAI.Database.Audits;

/// <summary>
/// 删除审计属性.
/// </summary>
public interface IDeleteAudited : IModificationAudited
{
    /// <summary>
    /// 是否删除.
    /// </summary>
    [Required]
    bool IsDeleted { get; set; }
}
