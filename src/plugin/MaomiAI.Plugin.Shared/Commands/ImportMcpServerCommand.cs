// <copyright file="ImportMcpServerCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Plugin.Shared.Commands;

/// <summary>
/// 导入 mcp 服务.
/// </summary>
public class ImportMcpServerCommand : IRequest<IdResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    /// <summary>
    /// 服务地址.
    /// </summary>
    public string ServerUrl { get; init; }

    /// <summary>
    /// json 字典.
    /// </summary>
    public string Header { get; init; }

    /// <summary>
    /// Query 字典.
    /// </summary>
    public string Query { get; init; }
}

/// <summary>
/// 修改分组信息.
/// </summary>
public class UpdatePluginInfoCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    public Guid GroupId { get; init; }

    public string Name { get; init; }

    public string ServerUrl { get; init; }
    public string Description { get; init; }

    /// <summary>
    /// json 字典.
    /// </summary>
    public string Header { get; init; }

    /// <summary>
    /// Query 字典.
    /// </summary>
    public string Query { get; init; }
}

/// <summary>
/// 删除分组.
/// </summary>
public class DeletePluginGroupCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    public Guid GroupId { get; init; }
}