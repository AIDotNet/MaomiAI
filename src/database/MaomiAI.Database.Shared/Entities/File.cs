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
    public string ObjectKey { get; set; } = null!;

    /// <summary>
    /// 允许公开访问，公有文件不带路径.
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// 文件md5值.
    /// </summary>
    public string FileMd5 { get; set; } = null!;

    /// <summary>
    /// 文件大小.
    /// </summary>
    public long FileSize { get; set; }

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
    /// 更新用户id.
    /// </summary>
    public Guid UpdateUserId { get; set; }

    /// <summary>
    /// 已上传文件.
    /// </summary>
    public bool IsUpload { get; set; }

    /// <summary>
    /// 文件类型.
    /// </summary>
    public string ContentType { get; set; } = null!;
}
