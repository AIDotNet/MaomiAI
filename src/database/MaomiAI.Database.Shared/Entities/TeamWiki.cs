using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 知识库.
/// </summary>
public partial class TeamWikiEntity : IFullAudited
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; set; }

    /// <summary>
    /// 知识库名称.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 知识库描述.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 绑定的向量模型id.
    /// </summary>
    public Guid ModelId { get; set; }

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
    /// 知识库详细介绍.
    /// </summary>
    public string Markdown { get; set; } = null!;

    /// <summary>
    /// 公开使用，所有人不需要加入团队即可使用此知识库.
    /// </summary>
    public bool IsPublic { get; set; }
}
