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
/// 获取插件分组列表.
/// </summary>
public class QueryPluginGroupListCommand : IRequest<QueryPluginGroupListCommandResponse>
{
    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 分组id，如果筛选 分组id，则只会返回一个结果.
    /// </summary>
    public Guid? GroupId { get; init; }
}
