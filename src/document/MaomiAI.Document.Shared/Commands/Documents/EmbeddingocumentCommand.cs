// <copyright file="EmbeddingocumentCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Document.Shared.Commands.Documents;

/// <summary>
/// 向量化文档.
/// </summary>
public class EmbeddingocumentCommand : IRequest<EmptyCommandResponse>
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
    public Guid DocumentId { get; init; }

    /// <summary>
    /// 文本分割方法，暂时不支持使用.
    /// </summary>
    public string SplitMethod { get; set; } = default!;

    /// <summary>
    /// 每个段落最大 token 数量.
    /// </summary>
    public int MaxTokensPerParagraph { get; set; } = 1000;

    /// <summary>
    /// 块之间重叠令牌的数量.
    /// </summary>
    public int OverlappingTokens { get; set; } = 100;

    /// <summary>
    /// 统计 tokens 数量的算法 支持: "p50k", "cl100k", "o200k".
    /// </summary>
    public string Tokenizer { get; set; } = "p50k";
}