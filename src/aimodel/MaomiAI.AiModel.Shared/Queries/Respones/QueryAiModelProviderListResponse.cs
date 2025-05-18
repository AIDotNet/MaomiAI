// <copyright file="QueryAiModelProviderListResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;

namespace MaomiAI.AiModel.Shared.Queries.Respones;

/// <summary>
/// AI 模型供应商和已添加的ai模型数量列表.
/// </summary>
public class QueryAiModelProviderListResponse
{
    /// <summary>
    /// AI 服务商列表，{ai服务提供商,模型数量}.
    /// </summary>
    public IReadOnlyCollection<QueryAiModelProviderCount> Providers { get; init; } = new List<QueryAiModelProviderCount>();
}
