// <copyright file="PromptItem.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Prompt.Models;

/// <summary>
/// 查询提示词详细信息.
/// </summary>
public class PromptItem : AuditsInfo
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 名称.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 描述.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// 助手设定,markdown.
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// 标签，使用逗号&quot;,&quot;分割多个标签值.
    /// </summary>
    public IReadOnlyCollection<string> Tags { get; set; } = default!;

    /// <summary>
    /// 头像路径.
    /// </summary>
    public string AvatarPath { get; set; } = default!;

    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid TeamId { get; set; }

    /// <summary>
    /// 团队名称.
    /// </summary>
    public string TeamName { get; init; } = default!;

    /// <summary>
    /// 提示词类型.
    /// </summary>
    public PromptType PromptType { get; set; }
}