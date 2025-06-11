using System;
using System.Collections.Generic;
using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 插件分组.
/// </summary>
public partial class TeamPluginGroupEntity : IFullAudited
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 分组名称.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 自定义服务器地址，mcp导入后无法修改.
    /// </summary>
    public string Server { get; set; } = default!;

    /// <summary>
    /// 自定义header头.
    /// </summary>
    public string Header { get; set; } = default!;

    /// <summary>
    /// 自定义header头.
    /// </summary>
    public string Query { get; set; } = default!;

    /// <summary>
    /// 类型，mcp或openapi或system.
    /// </summary>
    public int Type { get; set; }

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
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; set; }

    /// <summary>
    /// 描述.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// 文件id，mcp不需要填写.
    /// </summary>
    public Guid OpenapiFileId { get; set; }

    /// <summary>
    /// openapi文件名称.
    /// </summary>
    public string OpenapiFileName { get; set; } = default!;
}
