// <copyright file="QueryWikiDetailInfoCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Shared.Queries.Response;
using MediatR;

namespace MaomiAI.Document.Shared.Queries;

/// <summary>
/// 查询知识库详细信息.
/// </summary>
public class QueryWikiDetailInfoCommand : IRequest<QueryWikiDetailInfoResponse>
{
    public Guid WikiId { get; init; }
}
