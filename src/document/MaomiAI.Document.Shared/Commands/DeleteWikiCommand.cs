// <copyright file="DeleteWikiCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Document.Shared.Commands;

/// <summary>
/// 删除 Wiki.
/// </summary>
public class DeleteWikiCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 团队id.
    /// </summary>
    public Guid WikiId { get; init; }
}
