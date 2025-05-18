// <copyright file="UpdateWikiConfigCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Shared.Models;
using MaomiAI.Document.Shared.Queries.Response;
using MediatR;

namespace MaomiAI.Document.Shared.Commands;

/// <summary>
/// 更新知识库设置信息.
/// </summary>
public class UpdateWikiConfigCommand : IRequest<EmptyCommandResponse>
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
    /// 知识库配置.
    /// </summary>
    public WikiConfig Config { get; init; } = default!;
}
