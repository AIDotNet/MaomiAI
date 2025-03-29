// <copyright file="SystemOptions.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra;

/// <summary>
/// 文件存储选项.
/// </summary>
public class SystemStoreOption
{
    /// <summary>
    /// 节点.
    /// </summary>
    public string Endpoint { get; init; } = string.Empty;

    /// <summary>
    /// 存储桶.
    /// </summary>
    public string Bucket { get; init; } = string.Empty;

    /// <summary>
    /// AccessKeyId.
    /// </summary>
    public string AccessKeyId { get; init; } = string.Empty;

    /// <summary>
    /// AccessKeySecret.
    /// </summary>
    public string AccessKeySecret { get; init; } = string.Empty;
}