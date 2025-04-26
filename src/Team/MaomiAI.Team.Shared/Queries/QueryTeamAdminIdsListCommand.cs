// <copyright file="QueryTeamAdminIdsListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MaomiAI.Team.Shared.Queries;

/// <summary>
/// 检查用户是否是团队管理员的查询.
/// </summary>
public class QueryTeamAdminIdsListCommand : IRequest<TeamAdminListIdsResponse>
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    [Required]
    public Guid TeamId { get; init; }
}
