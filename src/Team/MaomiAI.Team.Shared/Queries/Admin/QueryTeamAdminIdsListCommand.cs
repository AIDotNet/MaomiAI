// <copyright file="QueryTeamAdminIdsListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MaomiAI.Team.Shared.Queries.Admin;

/// <summary>
/// 查询团队管理员id列表.
/// </summary>
public class QueryTeamAdminIdsListCommand : IRequest<TeamAdminListIdsResponse>
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    [Required]
    public Guid TeamId { get; init; }
}