﻿// <copyright file="QueryFileDownloadUrlCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Enums;
using MediatR;

namespace MaomiAI.Store.Queries;

/// <summary>
/// 查询文件下载地址.
/// </summary>
public class QueryFileDownloadUrlCommand : IRequest<QueryFileDownloadUrlCommandResponse>
{
    /// <summary>
    /// 文件可见性
    /// </summary>
    public FileVisibility Visibility { get; init; }

    /// <summary>
    /// 过期时间.
    /// </summary>
    public TimeSpan ExpiryDuration { get; init; }

    /// <summary>
    /// 对象列表.
    /// </summary>
    public IReadOnlyCollection<string> ObjectKeys { get; init; } = new List<string>();
}
