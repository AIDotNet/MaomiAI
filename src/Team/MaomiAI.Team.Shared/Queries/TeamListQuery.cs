// <copyright file="GetTeamsQuery.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Models;
using MediatR;

namespace MaomiAI.Team.Shared.Queries;

/// <summary>
/// 获取团队列表查询.
/// </summary>
public class TeamListQuery : PagedParamter, IRequest<PagedResult<TeamDto>>
{
    /// <summary>
    /// 查询关键字.
    /// </summary>
    public string? Keyword { get; set; }
}