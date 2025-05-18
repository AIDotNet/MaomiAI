// <copyright file="SetDefaultAiModelCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;
using MediatR;

namespace MaomiAI.AiModel.Shared.Commands;

/// <summary>
/// 设置某个功能模型使用的 ai 模型.
/// </summary>
public class SetDefaultAiModelCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// AI 模型的功能.
    /// </summary>
    public AiModelType AiModelType { get; init; }

    /// <summary>
    /// 模型 id.
    /// </summary>
    public Guid ModelId { get; init; }
}
