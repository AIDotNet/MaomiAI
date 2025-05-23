﻿// <copyright file="QueryWikiConfigCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Document.Shared.Queries.Response;

public class QueryWikiConfigCommandResponse : AuditsInfo
{
    /// <summary>
    /// 知识库id.
    /// </summary>
    public Guid WikiId { get; set; }

    /// <summary>
    /// 指定进行文档向量化的模型.
    /// </summary>
    public Guid EmbeddingModelId { get; set; }

    public DateTimeOffset CreateTime { get; set; }

    public DateTimeOffset UpdateTime { get; set; }

    /// <summary>
    /// 创建者id.
    /// </summary>
    public Guid CreateUserId { get; set; }

    /// <summary>
    /// 更新人id.
    /// </summary>
    public Guid UpdateUserId { get; set; }

    /// <summary>
    /// 锁定配置，锁定后不能再修改.
    /// </summary>
    public bool IsLock { get; set; }

    /// <summary>
    /// 分词器.
    /// </summary>
    public string EmbeddingModelTokenizer { get; set; } = default!;

    /// <summary>
    /// 维度，跟模型有关.
    /// </summary>
    public int EmbeddingDimensions { get; set; }

    /// <summary>
    /// 批处理大小.
    /// </summary>
    public int EmbeddingBatchSize { get; set; }

    /// <summary>
    /// 最大重试次数.
    /// </summary>
    public int MaxRetries { get; set; }
}