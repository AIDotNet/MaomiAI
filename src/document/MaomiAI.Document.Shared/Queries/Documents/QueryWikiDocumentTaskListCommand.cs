// <copyright file="QueryWikiDocumentTaskListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Shared.Queries.Documents.Responses;
using MediatR;

namespace MaomiAI.Document.Shared.Queries.Documents;

/// <summary>
/// 查询文档任务列表.
/// </summary>
public class QueryWikiDocumentTaskListCommand : IRequest<IReadOnlyCollection<WikiDocumentTaskItem>>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 知识库id.
    /// </summary>
    public Guid WikiId { get; set; }

    /// <summary>
    /// 文档id.
    /// </summary>
    public Guid DocumentId { get; set; }
}
