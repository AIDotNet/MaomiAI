// <copyright file="QueryWikiFileListItem.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Document.Shared.Queries.Response;

public class QueryWikiDocumentListItem : AuditsInfo
{
    public Guid DocumentId { get; init; }
    public string FileName { get; init; }
    public long FileSize { get; init; }
    public string ContentType { get; init; }

    /// <summary>
    /// 是否有向量化内容.
    /// </summary>
    public bool Embedding { get; init; }
}