// <copyright file="SystemOptions.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra;

/// <summary>
/// 系统配置.
/// </summary>
public class SystemOptions
{
    /// <summary>
    /// 服务访问地址.
    /// </summary>
    public string Server { get; init; } = string.Empty;

    /// <summary>
    /// RSA 私钥路径.
    /// </summary>
    public string RsaPath { get; init; } = string.Empty;

    /// <summary>
    /// 系统数据库类型.
    /// </summary>
    public string DBType { get; init; } = string.Empty;

    /// <summary>
    /// 系统数据库连接字符串.
    /// </summary>
    public string Database { get; init; } = string.Empty;

    // 向量数据库

    // s3 静态资源存储
    // s3 文档存储
}
