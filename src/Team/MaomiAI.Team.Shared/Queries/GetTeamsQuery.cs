// <copyright file="GetTeamsQuery.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Team.Shared.Models;
using MaomiAI.User.Shared.Models;

using MediatR;

namespace MaomiAI.Team.Shared.Queries;

/// <summary>
/// 获取团队列表查询.
/// </summary>
public class GetTeamsQuery : IRequest<PagedResult<TeamDto>>
{
    private int _page = 1;
    private int _pageSize = 10;
    private string? _keyword;

    /// <summary>
    /// 页码，从1开始.
    /// </summary>
    public int Page
    {
        get => _page;
        set => _page = Math.Max(1, value);
    }

    /// <summary>
    /// 每页大小.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Clamp(value, 1, 100); // 限制每页最大数量为100
    }

    /// <summary>
    /// 关键词（团队名称、描述）.
    /// </summary>
    public string? Keyword
    {
        get => _keyword;
        set => _keyword = !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;
    }

    /// <summary>
    /// 状态过滤.
    /// </summary>
    public bool? Status { get; set; }
}