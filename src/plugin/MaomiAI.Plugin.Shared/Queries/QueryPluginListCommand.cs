// <copyright file="QueryPluginGroupListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MaomiAI.Plugin.Shared.Queries.Responses;
using MediatR;

namespace MaomiAI.Plugin.Shared.Queries;

/// <summary>
/// 获取插件列表.
/// </summary>
public class QueryPluginListCommand : IRequest<QueryPluginListCommandResponse>
{
    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; init; }

    public Guid? GroupId { get; init; }

    public IReadOnlyCollection<Guid> PluginIds { get; init; } = new List<Guid>();
}
