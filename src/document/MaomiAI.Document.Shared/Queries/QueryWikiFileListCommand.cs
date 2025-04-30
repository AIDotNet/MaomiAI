// <copyright file="QueryWikiFileListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Shared.Queries.Response;
using MediatR;

namespace MaomiAI.Document.Shared.Queries;

/// <summary>
/// 查询 wiki 文件列表.
/// </summary>
public class QueryWikiFileListCommand : PagedParamter, IRequest<QueryWikiFileListResponse>
{
    public Guid TeamId { get; init; } = default!;
    public Guid WikiId { get; init; } = default!;

    public string? Search { get; init; } = default!;
}
