// <copyright file="CheckFileExistCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Enums;
using MaomiAI.Store.Queries.Response;
using MediatR;

namespace MaomiAI.Store.Queries;

/// <summary>
/// 检查文件是否存在
/// </summary>
public class CheckFileExistCommand : IRequest<CheckFileExistResponse>
{
    /// <summary>
    /// 文件可见性，区分私有存储桶和公共存储桶.
    /// </summary>
    public FileVisibility Visibility { get; init; }

    /// <summary>
    /// 文件id.
    /// </summary>
    public Guid? FileId { get; init; }

    /// <summary>
    /// MD5
    /// </summary>
    public string? MD5 { get; init; }

    /// <summary>
    /// Key.
    /// </summary>
    public string? Key { get; init; }
}
