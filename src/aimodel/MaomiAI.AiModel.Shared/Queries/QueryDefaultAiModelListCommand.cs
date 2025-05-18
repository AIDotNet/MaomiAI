// <copyright file="QueryDefaultAiModelListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;
using MaomiAI.AiModel.Shared.Queries.Respones;
using MediatR;

namespace MaomiAI.AiModel.Shared.Queries;

/// <summary>
/// 查询供应商模型默认列表，获取在某个功能需求下默认使用的模型.
/// </summary>
public class QueryDefaultAiModelListCommand : IRequest<QueryDefaultAiModelListResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; } = default!;

    /// <summary>
    /// 模型类型.
    /// </summary>
    public AiModelType AiModelType { get; init; } = default!;
}
