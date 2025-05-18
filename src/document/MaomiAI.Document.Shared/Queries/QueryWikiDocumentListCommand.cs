// <copyright file="QueryWikiFileListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Shared.Queries.Response;
using MediatR;

namespace MaomiAI.Document.Shared.Queries;

/// <summary>
/// 查询 wiki 文件列表.
/// </summary>
public class QueryWikiDocumentListCommand : PagedParamter, IRequest<QueryWikiDocumentListResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 知识库 id.
    /// </summary>
    public Guid WikiId { get; init; }

    /// <summary>
    /// 筛选文件名称.
    /// </summary>
    public string? Query { get; init; } = default!;
}
