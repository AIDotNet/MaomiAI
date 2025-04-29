// <copyright file="QueryAiModelDefaultConfigurationsCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;
using MaomiAI.AiModel.Shared.Queries.Respones;
using MediatR;

namespace MaomiAI.AiModel.Shared.Queries;

/// <summary>
/// 查询供应商模型默认配置.
/// </summary>
public class QueryAiModelDefaultConfigurationsCommand : IRequest<QueryAiModelDefaultConfigurationsResponse>
{
    /// <summary>
    /// 模型名称或id.
    /// </summary>
    public string ModelId { get; init; } = default!;

    /// <summary>
    /// AI 服务商.
    /// </summary>
    public AiProvider Provider { get; init; }
}
