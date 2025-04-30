// <copyright file="CreateWikiCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Document.Shared.Commands;

/// <summary>
/// 创建知识库.
/// </summary>
public class CreateWikiCommand : IRequest<IdResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; } = default!;

    /// <summary>
    /// 团队名称.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 团队描述.
    /// </summary>
    public string Description { get; set; } = null!;
}
