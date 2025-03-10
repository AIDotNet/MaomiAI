// <copyright file="LoginCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using MaomiAI.User.Shared.Models;

using MediatR;

namespace MaomiAI.User.Shared.Commands;

/// <summary>
/// 登录命令.
/// </summary>
public class LoginCommand : IRequest<LoginResult>
{
    /// <summary>
    /// 用户名或邮箱.
    /// </summary>
    [Required]
    public string Username { get; set; } = null!;

    /// <summary>
    /// 密码.
    /// </summary>
    [Required]
    public string Password { get; set; } = null!;
} 