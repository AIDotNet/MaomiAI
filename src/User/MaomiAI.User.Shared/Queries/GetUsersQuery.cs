// <copyright file="GetUsersQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using MaomiAI.User.Shared.Models;

using MediatR;

namespace MaomiAI.User.Shared.Queries;

/// <summary>
/// 获取用户列表查询.
/// </summary>
public class GetUsersQuery : IRequest<PagedResult<UserDto>>
{
    /// <summary>
    /// 页码，从1开始.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页大小.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// 关键词（用户名、昵称、邮箱）.
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    /// 状态过滤.
    /// </summary>
    public bool? Status { get; set; }
}