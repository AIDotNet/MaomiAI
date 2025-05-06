// <copyright file="QueryPublicFilePathCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Queries.Response;
using MediatR;

namespace MaomiAI.Store.Queries;

/// <summary>
/// 获取文件的访问路径，只支持公有文件.
/// </summary>
public class QueryPublicFilePathCommand : IRequest<QueryPublicFilePathResponse>
{
    /// <summary>
    /// 文件 id.
    /// </summary>
    public Guid? FileId { get; init; }

    /// <summary>
    /// md5 值.
    /// </summary>
    public string? MD5 { get; init; }

    /// <summary>
    ///  object key.
    /// </summary>
    public string? Key { get; init; }
}
