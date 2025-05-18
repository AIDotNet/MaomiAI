// <copyright file="SearchWikiDocumentTextCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Core.Handlers.Responses;
using MediatR;

namespace MaomiAI.Document.Shared.Queries.Documents;

/// <summary>
/// 搜索知识库文档分片.
/// </summary>
public class SearchWikiDocumentTextCommand : IRequest<SearchWikiDocumentTextCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    ///  知识库 id.
    /// </summary>
    public Guid WikiId { get; init; } = default!;

    /// <summary>
    /// 文档id，不设置时搜索整个知识库.
    /// </summary>
    public Guid? DocumentId { get; set; }

    /// <summary>
    /// 搜索文本，区配文本.
    /// </summary>
    public string? Query { get; init; }
}
