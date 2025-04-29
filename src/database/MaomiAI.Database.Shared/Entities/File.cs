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

    /// <summary>
    /// 已上传文件.
    /// </summary>
    public bool IsUpload { get; set; }

    /// <summary>
    /// 文件类型.
    /// </summary>
    public string ContentType { get; set; } = null!;
}
