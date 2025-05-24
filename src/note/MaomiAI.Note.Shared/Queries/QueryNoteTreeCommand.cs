// <copyright file="QueryNoteTreeCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Note.Queries;

public class QueryNoteTreeCommand : IRequest<QueryNoteTreeCommandResponse>
{
    /// <summary>
    /// 是否包括摘要.
    /// </summary>
    public bool IncludeSummary { get; init; }

    /// <summary>
    /// 指定父级.
    /// </summary>
    public Guid? ParantId { get; init; }

    /// <summary>
    /// 指定搜索词，默认只搜索标题.
    /// </summary>
    public string? Search { get; init; }

    /// <summary>
    /// 是否搜索笔记内容.
    /// </summary>
    public bool SearchContent { get; init; }
}

public class QueryNoteTreeCommandResponse
{
    public IReadOnlyCollection<NoteTreeItem> Notes { get; set; } = new List<NoteTreeItem>();
}

public class NoteTreeItem
{
    public Guid NoteId { get; set; }
    public Guid ParentId { get; set; }
    public string Title { get; set; } = default!;
    public string TitleEmoji { get; set; } = default!;
    public string Summary { get; set; } = string.Empty;

    public List<NoteTreeItem> Children { get; set; } = new List<NoteTreeItem>();
}