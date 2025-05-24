using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 笔记.
/// </summary>
public partial class NoteEntity : IFullAudited
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 自增id，用于标识父级等.
    /// </summary>
    public int NoteId { get; set; }

    /// <summary>
    /// 笔记标题.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// 图标.
    /// </summary>
    public string TitleEmoji { get; set; } = default!;

    /// <summary>
    /// 总结.
    /// </summary>
    public string Summary { get; set; } = default!;

    /// <summary>
    /// 笔记内容.
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// 父级id.
    /// </summary>
    public Guid ParentId { get; set; }

    /// <summary>
    /// 父级路径.
    /// </summary>
    public string ParentPath { get; set; } = default!;

    /// <summary>
    /// 当前路径，等于父级路径加上自己的note_id.
    /// </summary>
    public string CurrentPath { get; set; } = default!;

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
    /// 开启共享.
    /// </summary>
    public bool IsShared { get; set; }
}
