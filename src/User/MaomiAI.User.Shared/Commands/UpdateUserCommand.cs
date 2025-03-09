// <copyright file="UpdateUserCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace MaomiAI.User.Shared.Commands;

/// <summary>
/// 更新用户命令.
/// </summary>
public class UpdateUserCommand : IRequest
{
    /// <summary>
    /// 用户ID.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// 邮箱.
    /// </summary>
    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

    /// <summary>
    /// 昵称.
    /// </summary>
    [StringLength(50)]
    public string? NickName { get; set; }

    /// <summary>
    /// 头像URL.
    /// </summary>
    [StringLength(500)]
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// 手机号.
    /// </summary>
    [Phone]
    [StringLength(20)]
    public string? Phone { get; set; }
} 