// <copyright file="LoginResult.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.User.Shared.Models;

/// <summary>
/// 登录结果.
/// </summary>
public class LoginResult
{
    /// <summary>
    /// 用户ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 用户名.
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 访问令牌.
    /// </summary>
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// 令牌类型.
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// 过期时间（秒）.
    /// </summary>
    public int ExpiresIn { get; set; }
} 