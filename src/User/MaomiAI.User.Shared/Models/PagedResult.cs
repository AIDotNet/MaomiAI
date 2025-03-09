// <copyright file="PagedResult.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.User.Shared.Models;

/// <summary>
/// 分页结果.
/// </summary>
/// <typeparam name="T">结果元素类型.</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// 初始化一个新的<see cref="PagedResult{T}"/>实例.
    /// </summary>
    /// <param name="items">项目集合.</param>
    /// <param name="total">总数.</param>
    /// <param name="page">当前页.</param>
    /// <param name="pageSize">每页大小.</param>
    public PagedResult(IEnumerable<T> items, long total, int page, int pageSize)
    {
        Items = items;
        Total = total;
        Page = page;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(total / (double)pageSize);
    }

    /// <summary>
    /// 项目集合.
    /// </summary>
    public IEnumerable<T> Items { get; }

    /// <summary>
    /// 总数.
    /// </summary>
    public long Total { get; }

    /// <summary>
    /// 当前页.
    /// </summary>
    public int Page { get; }

    /// <summary>
    /// 每页大小.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// 总页数.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// 是否有上一页.
    /// </summary>
    public bool HasPrevious => Page > 1;

    /// <summary>
    /// 是否有下一页.
    /// </summary>
    public bool HasNext => Page < TotalPages;
} 