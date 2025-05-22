// <copyright file="QueryPluginGroupListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MaomiAI.Plugin.Shared.Queries.Responses;
using MediatR;

namespace MaomiAI.Plugin.Shared.Queries;

public class QueryPluginListItem
{
    public Guid Id { get; init; }

    public Guid GroupId { get; init; }
    public string GroupName { get; init; } = default!;

    /// <summary>
    /// 名称.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 描述.
    /// </summary>
    public string Summary { get; init; }

    /// <summary>
    /// 路径.
    /// </summary>
    public string Path { get; init; }
}