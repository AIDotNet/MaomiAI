// <copyright file="QueryAiModelListCommandResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;

namespace MaomiAI.AiModel.Shared.Queries.Respones;

/// <summary>
/// Ai 模型列表.
/// </summary>
public class QueryAiModelListCommandResponse
{
    /// <summary>
    /// AI 模型列表.
    /// </summary>
    public IReadOnlyCollection<AiNotKeyEndpoint> AiModels { get; init; } = new List<AiNotKeyEndpoint>();
}