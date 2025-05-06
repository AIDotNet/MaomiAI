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
    /// 模型功能AiModelFunction.
    /// </summary>
    public int AiModelFunction { get; set; }

    /// <summary>
    /// ai供应商AiProvider.
    /// </summary>
    public int AiProvider { get; set; }

    /// <summary>
    /// api服务端点.
    /// </summary>
    public string Endpoint { get; set; } = null!;

    /// <summary>
    /// 模型id.
    /// </summary>
    public string ModeId { get; set; } = null!;

    /// <summary>
    /// 部署名称.
    /// </summary>
    public string DeploymentName { get; set; } = null!;

    /// <summary>
    /// key.
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// 是否支持图片.
    /// </summary>
    public bool IsSupportImg { get; set; }

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
    public bool IsSupportFunctionCall { get; set; }

    /// <summary>
    /// 名字.
    /// </summary>
    public string Name { get; set; } = null!;
}
