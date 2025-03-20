// <copyright file="JwtOptions.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Services
{
    /// <summary>
    /// JWT配置选项.
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// 配置节点名称.
        /// </summary>
        public const string SectionName = "Jwt";

        /// <summary>
        /// 密钥.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// 发行者.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// 接收者.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// 过期时间（分钟）.
        /// </summary>
        public int ExpirationMinutes { get; set; } = 60;
    }
}