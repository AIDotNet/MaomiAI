// <copyright file="CreatePromptCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Prompt.Commands;

/// <summary>
/// 公开提示词.
/// </summary>
public class PublishPromptCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 提示词 id.
    /// </summary>
    public Guid PromptId { get; init; }

    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 是否公开.
    /// </summary>
    public bool IsPublic { get; init; }
}
