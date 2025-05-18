// <copyright file="CancalWikiDocumentTaskCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Document.Shared.Commands.Documents;

/// <summary>
/// 取消文档处理任务.
/// </summary>
public class CancalWikiDocumentTaskCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 知识库id.
    /// </summary>
    public Guid WikiId { get; set; }

    /// <summary>
    /// 文档id.
    /// </summary>
    public Guid DocumentId { get; set; }

    /// <summary>
    /// 任务 id.
    /// </summary>
    public Guid TaskId { get; set; } = default!;
}