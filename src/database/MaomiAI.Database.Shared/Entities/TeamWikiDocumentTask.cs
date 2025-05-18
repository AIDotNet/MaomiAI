using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 知识库文档处理任务.
/// </summary>
public partial class TeamWikiDocumentTaskEntity : IFullAudited
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
    /// 文档id.
    /// </summary>
    public Guid DocumentId { get; set; }

    /// <summary>
    /// 文件id.
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// 任务标识，用来判断要执行的任务是否一致.
    /// </summary>
    public string TaskTag { get; set; } = default!;

    /// <summary>
    /// 任务状态.
    /// </summary>
    public int State { get; set; }

    /// <summary>
    /// 当前使用的模型id，默认跟知识库配置一致.
    /// </summary>
    public Guid ModelId { get; set; }

    /// <summary>
    /// 执行信息.
    /// </summary>
    public string Message { get; set; } = default!;

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
    /// 每段最大token数量.
    /// </summary>
    public int MaxTokensPerParagraph { get; set; }

    /// <summary>
    /// 重叠的token数量.
    /// </summary>
    public int OverlappingTokens { get; set; }

    /// <summary>
    /// 分词器.
    /// </summary>
    public string Tokenizer { get; set; } = default!;
}
