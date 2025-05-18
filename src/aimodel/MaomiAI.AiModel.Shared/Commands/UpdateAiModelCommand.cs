// <copyright file="UpdateAiModelCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;
using MediatR;

namespace MaomiAI.AiModel.Shared.Commands;

/// <summary>
/// 修改 AI 模型.
/// </summary>
public class UpdateAiModelCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// AI 模型 id.
    /// </summary>
    public Guid ModelId { get; init; }

    /// <summary>
    /// AI 端点.
    /// </summary>
    public AiEndpoint Endpoint { get; init; } = default!;
}
