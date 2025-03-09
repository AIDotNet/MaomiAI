// <copyright file="UserDto.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.User.Shared.Models;

/// <summary>
/// 用户数据传输对象.
/// </summary>
public class UserDto
{
    /// <summary>
    /// 用户ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 用户名.
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 邮箱.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// 昵称.
    /// </summary>
    public string NickName { get; set; } = null!;

    /// <summary>
    /// 头像URL.
    /// </summary>
    public string AvatarUrl { get; set; } = null!;

    /// <summary>
    /// 手机号.
    /// </summary>
    public string Phone { get; set; } = null!;

    /// <summary>
    /// 状态：true-正常，false-禁用.
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public DateTimeOffset CreateTime { get; set; }

    /// <summary>
    /// 更新时间.
    /// </summary>
    public DateTimeOffset UpdateTime { get; set; }
} 