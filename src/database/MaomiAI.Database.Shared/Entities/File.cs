using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 文件列表.
/// </summary>
public partial class FileEntity : IFullAudited
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 文件名称.
    /// </summary>
    public string FileName { get; set; } = null!;

    /// <summary>
    /// 文件路径.
    /// </summary>
    public string Path { get; set; } = null!;

    /// <summary>
    /// 允许公开访问.
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// 该文件属于哪个模块.
    /// </summary>
    public string SourceType { get; set; } = null!;

    /// <summary>
    /// 文件md5值.
    /// </summary>
    public string FileMd5 { get; set; } = null!;

    /// <summary>
    /// 文件大小.
    /// </summary>
    public long FileSize { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset CreateTime { get; set; }

    public DateTimeOffset UpdateTime { get; set; }

    public Guid CreateUserId { get; set; }

    public Guid UpdateUserId { get; set; }
}
