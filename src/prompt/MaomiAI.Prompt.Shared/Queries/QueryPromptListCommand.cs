// <copyright file="QueryPromptListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MaomiAI.Prompt.Queries.Responses;
using MediatR;

namespace MaomiAI.Prompt.Queries;

/// <summary>
/// 查询能看到的提示词列表.
/// </summary>
public class QueryPromptListCommand : IRequest<QueryPromptListCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid? TeamId { get; init; }
}
