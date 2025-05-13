// <copyright file="SetWikiDefaultModelCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Document.Shared.Commands;

/// <summary>
/// 设置知识库默认向量化模型.
/// </summary>
public class SetWikiDefaultModelCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 知识库 id.
    /// </summary>
    public Guid WikiId { get; init; }

    /// <summary>
    /// 模型id.
    /// </summary>
    public Guid ModelId { get; init; }
}
