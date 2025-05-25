// <copyright file="QueryPromptListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MaomiAI.Prompt.Models;
using MaomiAI.Prompt.Queries.Responses;
using MediatR;

namespace MaomiAI.Prompt.Queries;

/// <summary>
/// 查询能看到的提示词列表.
/// </summary>
public class QueryPromptListCommand : IRequest<QueryPromptListCommandResponse>
{
    /// <summary>
    /// 团队 id，如果不填写，则返回所有公开的提示词.
    /// </summary>
    public Guid? TeamId { get; init; }

    /// <summary>
    /// 指定获取提示词，填写 PromptId 后，返回的提示词才会带上 Content.
    /// </summary>
    public Guid? PromptId { get; init; }

    /// <summary>
    /// 筛选分类.
    /// </summary>
    public PromptType? PromptType { get; init; }
}
