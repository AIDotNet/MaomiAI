// <copyright file="QueryAiModelProviderListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Queries.Respones;
using MediatR;

namespace MaomiAI.AiModel.Shared.Queries;

/// <summary>
/// 查询 ai 模型供应商和已添加的ai模型数量列表.
/// </summary>
public class QueryAiModelProviderListCommand : IRequest<QueryAiModelProviderListResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }
}
