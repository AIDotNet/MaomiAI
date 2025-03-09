// <copyright file="ToggleUserStatusCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace MaomiAI.User.Shared.Commands;

/// <summary>
/// 切换用户状态命令.
/// </summary>
public class ToggleUserStatusCommand : IRequest
{
    /// <summary>
    /// 用户ID.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// 状态: true-启用, false-禁用.
    /// </summary>
    [Required]
    public bool Status { get; set; }
} 