// <copyright file="DeleteWikiDocumentCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Document.Shared.Commands.Documents;

/// <summary>
/// 删除知识库文档.
/// </summary>
public class DeleteWikiDocumentCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 知识库 id.
    /// </summary>
    public Guid WikiId { get; init; }

    /// <summary>
    /// 文档 id.
    /// </summary>
    public Guid DocumentId { get; init; }
}