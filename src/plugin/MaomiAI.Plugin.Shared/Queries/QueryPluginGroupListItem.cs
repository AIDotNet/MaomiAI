// <copyright file="QueryPluginGroupListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Plugin.Shared.Queries;

public class QueryPluginGroupListItem : AuditsInfo
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

    public string Query { get; set; } = default!;

    /// <summary>
    /// 类型，mcp或openapi或system.
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; set; }

    /// <summary>
    /// 描述.
    /// </summary>
    public string Description { get; set; } = default!;
}