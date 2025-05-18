using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// ai模型.
/// </summary>
public partial class TeamAiModelEntity : IFullAudited
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
    /// 模型类型，AiModelType.
    /// </summary>
    public string AiModelType { get; set; } = default!;

    /// <summary>
    /// ai供应商AiProvider.
    /// </summary>
    public string AiProvider { get; set; } = default!;

    /// <summary>
    /// api服务端点.
    /// </summary>
    public string Endpoint { get; set; } = default!;

    /// <summary>
    /// 部署名称.
    /// </summary>
    public string DeploymentName { get; set; } = default!;

    /// <summary>
    /// key.
    /// </summary>
    public string Key { get; set; } = default!;

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
    /// 支持function call.
    /// </summary>
    public bool FunctionCall { get; set; }

    /// <summary>
    /// 名字.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 最大文本输出token.
    /// </summary>
    public int TextOutput { get; set; }

    /// <summary>
    /// 向量的维度.
    /// </summary>
    public int MaxDimension { get; set; }

    /// <summary>
    /// 显示名称.
    /// </summary>
    public string DisplayName { get; set; } = default!;

    /// <summary>
    /// 上下文最大token数量.
    /// </summary>
    public int ContextWindowTokens { get; set; }

    /// <summary>
    /// 支持文件上传.
    /// </summary>
    public bool Files { get; set; }

    /// <summary>
    /// 支持图片输出.
    /// </summary>
    public bool ImageOutput { get; set; }

    /// <summary>
    /// 支持视觉.
    /// </summary>
    public bool Vision { get; set; }
}
