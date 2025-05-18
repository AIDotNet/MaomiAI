// <copyright file="QueryCanUpdateWikiCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Document.Shared.Queries;

/// <summary>
/// 用户是否有权限更新知识库.
/// </summary>
public class QueryCanUpdateWikiCommand : IRequest<bool>
{
    public Guid WikiId { get; init; }
    public Guid UserId { get; init; }
}
