// <copyright file="QueryWikiSimpleInfoCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Shared.Queries.Response;
using MediatR;

namespace MaomiAI.Document.Shared.Queries;

/// <summary>
/// 查询团队知识库列表.
/// </summary>
public class QueryTeamWikiListCommand : IRequest<ICollection<QueryWikiSimpleInfoResponse>>
{
    public Guid TeamId { get; init; }
}