// <copyright file="SystemOptions.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI;

/// <summary>
/// 系统配置选项.
/// </summary>
public class SystemOptions
{
    /// <summary>
    /// 服务器地址.
    /// </summary>
    public string Server { get; set; } = string.Empty;

    /// <summary>
    /// 数据库类型.
    /// </summary>
    public string DBType { get; set; } = "postgres";

    /// <summary>
    /// 数据库连接字符串.
    /// </summary>
    public string Database { get; set; } = string.Empty;
}