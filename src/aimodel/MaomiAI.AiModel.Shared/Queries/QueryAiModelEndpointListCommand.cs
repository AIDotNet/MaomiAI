// <copyright file="QueryAiModelEndpointListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;
using MaomiAI.AiModel.Shared.Queries.Respones;
using MediatR;

namespace MaomiAI.AiModel.Shared.Queries;

/// <summary>
/// 查询供应商下已配置的 ai 模型列表.
/// </summary>
public class QueryAiModelEndpointListCommand : IRequest<QueryAiModelEndpointListResponse>
{
    public Guid TeamId { get; init; }
    public AiProvider Provider { get; init; }
}
