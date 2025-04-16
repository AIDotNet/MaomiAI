// <copyright file="CheckTeamAdminQuery.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MaomiAI.Team.Shared.Queries;

/// <summary>
/// 检查用户是否是团队管理员的查询.
/// </summary>
public class QueryTeamAdminIdsListReuqest : IRequest<TeamAdminListIdsResponse>
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    [Required]
    public Guid TeamId { get; init; }
}

public class TeamAdminListIdsResponse
{
    /// <summary>
    /// 所有者 id.
    /// </summary>
    public Guid OwnId { get; init; }

    /// <summary>
    /// 管理员 id 列表.
    /// </summary>
    public IReadOnlyCollection<Guid> AdminIds { get; init; }
}
