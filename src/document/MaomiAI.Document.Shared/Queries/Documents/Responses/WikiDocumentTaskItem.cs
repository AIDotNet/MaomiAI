﻿// <copyright file="WikiDocumentTaskItem.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Shared.Models;

namespace MaomiAI.Document.Shared.Queries.Documents.Responses;

/// <summary>
/// 文档列表.
/// </summary>
public class WikiDocumentTaskItem : AuditsInfo
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; set; }

    /// <summary>
    /// 知识库id.
    /// </summary>
    public Guid WikiId { get; set; }

    /// <summary>
    /// 文档id.
    /// </summary>
    public Guid DocumentId { get; set; }

    /// <summary>
    /// 文件id.
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// 文件名称.
    /// </summary>
    public string FileName { get; init; }

    /// <summary>
    /// 文件大小.
    /// </summary>
    public long FileSize { get; init; }

    /// <summary>
    /// 文件类型.
    /// </summary>
    public string ContentType { get; init; }

    /// <summary>
    /// 任务标识，用来判断要执行的任务是否一致.
    /// </summary>
    public string TaskTag { get; set; } = default!;

    /// <summary>
    /// 任务状态.
    /// </summary>
    public FileEmbeddingState State { get; set; }

    /// <summary>
    /// 执行信息.
    /// </summary>
    public string Message { get; set; } = default!;

    /// <summary>
    /// 每段最大token数量.
    /// </summary>
    public int MaxTokensPerParagraph { get; set; }

    /// <summary>
    /// 重叠的token数量.
    /// </summary>
    public int OverlappingTokens { get; set; }

    /// <summary>
    /// 分词器.
    /// </summary>
    public string Tokenizer { get; set; } = default!;
}