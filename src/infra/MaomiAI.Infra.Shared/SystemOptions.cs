// <copyright file="SystemOptions.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra
{
    /// <summary>
    /// 系统配置.
    /// </summary>
    public class SystemOptions
    {
        /// <summary>
        /// 服务访问地址.
        /// </summary>
        public string Server { get; init; } = string.Empty;

        public string AES { get; init; } = string.Empty;

        /// <summary>
        /// 系统数据库类型.
        /// </summary>
        public string DBType { get; init; } = string.Empty;

        /// <summary>
        /// 系统数据库连接字符串.
        /// </summary>
        public string Database { get; init; } = string.Empty;

        // 向量数据库

        public ObjectStore PublicStore { get; init; }
        public ObjectStore PrivateStore { get; init; }

        public class ObjectStore
        {
            /// <summary>
            /// 静态资源存储类型，将会公开全部访问，可选：S3、Local.
            /// </summary>
            public string Type { get; init; } = string.Empty;

            /// <summary>
            /// Type = "S3" 时填写，静态资源存储配置.
            /// </summary>
            public StoreOption Options { get; init; }

            /// <summary>
            /// Type = "Local" 时填写，静态资源存储路径.
            /// </summary>
            public string Path { get; init; } = string.Empty;
        }

        public class StoreOption
        {
            public string Endpoint { get; init; } = string.Empty;
            public string Bucket { get; init; } = string.Empty;
            public string AccessKeyId { get; init; } = string.Empty;
            public string AccessKeySecret { get; init; } = string.Empty;
        }
    }
}