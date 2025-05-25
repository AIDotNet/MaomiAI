// <copyright file="CreatePromptCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MaomiAI.Prompt.Models;
using MediatR;

namespace MaomiAI.Prompt.Commands;

/// <summary>
/// 创建提示词.
/// </summary>
public class CreatePromptCommand : IRequest<IdResponse>
{
    /// <summary>
    /// 名称.
    /// </summary>
    public string Name { get; init; } = default!;

    /// <summary>
    /// 描述.
    /// </summary>
    public string Description { get; init; } = default!;

    /// <summary>
    /// 助手设定,markdown.
    /// </summary>
    public string Content { get; init; } = default!;

    /// <summary>
    /// 标签.
    /// </summary>
    public IReadOnlyCollection<string> Tags { get; init; } = new List<string>();

    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 分类.
    /// </summary>
    public PromptType PromptType { get; init; }
}
