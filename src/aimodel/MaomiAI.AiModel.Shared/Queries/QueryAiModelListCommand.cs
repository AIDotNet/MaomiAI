// <copyright file="QueryAiModelListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;
using MaomiAI.AiModel.Shared.Queries.Respones;
using MediatR;

namespace MaomiAI.AiModel.Shared.Queries;

/// <summary>
/// 查询模型列表.
/// </summary>
public class QueryAiModelListCommand : IRequest<QueryAiModelListCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// AI 模型类型.
    /// </summary>
    public AiProvider? Provider { get; init; }

    /// <summary>
    /// Ai 模型类型.
    /// </summary>
    public AiModelType? AiModelType { get; init; }
}
