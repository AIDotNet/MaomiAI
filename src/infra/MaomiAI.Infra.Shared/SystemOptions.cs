﻿// <copyright file="SystemOptions.cs" company="MaomiAI">
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
    /// 加密密钥.
    /// </summary>
    public string AES { get; init; } = string.Empty;

    /// <summary>
    /// 系统数据库类型.
    /// </summary>
    public string DBType { get; init; } = string.Empty;

    /// <summary>
    /// 系统数据库连接字符串.
    /// </summary>
    public string Database { get; init; } = string.Empty;

    /// <summary>
    /// Redis 连接字符串.
    /// </summary>
    public string Redis { get; init; } = string.Empty;

    /// <summary>
    /// 消息处理.
    /// </summary>
    public MessageOption MessageStore { get; init; } = new MessageOption();

    /// <summary>
    /// 公共文件存储.
    /// </summary>
    public SystemStoreOption PublicStore { get; init; } = default!;

    /// <summary>
    /// 私有文件存储.
    /// </summary>
    public SystemStoreOption PrivateStore { get; init; } = default!;

    /// <summary>
    /// 文档向量化存储.
    /// </summary>
    public DatabaseStore DocumentStore { get; init; } = new DatabaseStore();

    public class DatabaseStore
    {
        /// <summary>
        /// 系统数据库类型.
        /// </summary>
        public string DBType { get; init; } = string.Empty;

        /// <summary>
        /// 系统数据库连接字符串.
        /// </summary>
        public string Database { get; init; } = string.Empty;
    }


    /// <summary>
    /// 消息处理.
    /// </summary>
    public class MessageOption
    {
        /// <summary>
        /// 模式.
        /// </summary>
        public string Mode { get; init; } = "Local";

        /// <summary>
        /// RabbitMQ 连接字符串.
        /// </summary>
        public string RabbitMQ { get; init; } = string.Empty;
    }
}
