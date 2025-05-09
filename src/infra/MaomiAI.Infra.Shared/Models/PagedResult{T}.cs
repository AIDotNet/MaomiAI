// <copyright file="PagedResult.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra.Models;

/// <summary>
/// 分页结果.
/// </summary>
/// <typeparam name="T">结果元素类型.</typeparam>
public class PagedResult<T> : PagedParamter
{
    /// <summary>
    /// 项目集合.
    /// </summary>
    public IReadOnlyCollection<T> Items { get; init; }

    /// <summary>
    /// 总数.
    /// </summary>
    public int Total { get; init; }
}