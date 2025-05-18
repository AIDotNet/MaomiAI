// <copyright file="QueryWikiFileListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Shared.Queries.Response;
using MediatR;

namespace MaomiAI.Document.Shared.Queries.Documents;

/// <summary>
/// 查询单个文档信息.
/// </summary>
public class QueryWikiDocumentInfoCommand : IRequest<QueryWikiDocumentListItem>
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
    /// 文档 id.
    /// </summary>
    public Guid DocumentId { get; init; } = default!;
}