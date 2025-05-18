// <copyright file="QueryWikiSimpleInfoCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Shared.Queries.Response;
using MediatR;

namespace MaomiAI.Document.Shared.Queries;

/// <summary>
/// 获取知识库简单信息.
/// </summary>
public class QueryWikiSimpleInfoCommand : IRequest<QueryWikiSimpleInfoResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 知识库 id.
    /// </summary>
    public Guid WikiId { get; init; }
}
