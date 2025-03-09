// <copyright file="ChangePasswordCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace MaomiAI.User.Shared.Commands;

/// <summary>
/// 修改密码命令.
/// </summary>
public class ChangePasswordCommand : IRequest
{
    /// <summary>
    /// 用户ID.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// 旧密码.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string OldPassword { get; set; } = null!;

    /// <summary>
    /// 新密码.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; } = null!;

    /// <summary>
    /// 确认新密码.
    /// </summary>
    [Required]
    [Compare("NewPassword", ErrorMessage = "确认新密码与新密码不匹配")]
    public string ConfirmNewPassword { get; set; } = null!;
} 