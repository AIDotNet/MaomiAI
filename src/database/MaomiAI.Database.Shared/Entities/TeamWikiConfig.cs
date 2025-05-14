using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 团队知识库配置.
/// </summary>
public partial class TeamWikiConfigEntity : IFullAudited
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
    /// 知识库id.
    /// </summary>
    public Guid WikiId { get; set; }

    /// <summary>
    /// 指定进行文档向量化的模型.
    /// </summary>
    public Guid EmbeddingModelId { get; set; }

    /// <summary>
    /// 是否删除.
    /// </summary>
    public bool IsDeleted { get; set; }

    public DateTimeOffset CreateTime { get; set; }

    public DateTimeOffset UpdateTime { get; set; }

    /// <summary>
    /// 创建者id.
    /// </summary>
    public Guid CreateUserId { get; set; }

    /// <summary>
    /// 更新人id.
    /// </summary>
    public Guid UpdateUserId { get; set; }

    /// <summary>
    /// 锁定配置，锁定后不能再修改.
    /// </summary>
    public bool IsLock { get; set; }

    /// <summary>
    /// 分词器.
    /// </summary>
    public string EmbeddingModelTokenizer { get; set; }  = default!;

    /// <summary>
    /// 维度，跟模型有关.
    /// </summary>
    public int EmbeddingDimensions { get; set; }

    /// <summary>
    /// 批处理大小.
    /// </summary>
    public int EmbeddingBatchSize { get; set; }

    /// <summary>
    /// 最大重试次数.
    /// </summary>
    public int MaxRetries { get; set; }
}
