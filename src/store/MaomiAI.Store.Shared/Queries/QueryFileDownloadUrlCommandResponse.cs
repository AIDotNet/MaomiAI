// <copyright file="QueryFileDownloadUrlCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Enums;
using MediatR;

namespace MaomiAI.Store.Queries;

/// <summary>
/// 查询文件下载地址响应.
/// </summary>
public class QueryFileDownloadUrlCommandResponse
{
    /// <summary>
    /// 地址.
    /// </summary>
    public IReadOnlyDictionary<string, Uri> Urls { get; init; } = new Dictionary<string, Uri>();
}
