// <copyright file="QueryTeamMemberListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;

namespace MaomiAI.Team.Shared.Queries.Admin;

/// <summary>
/// 查询团队成员列表.
/// </summary>
public class QueryTeamMemberListCommand : PagedParamter, IRequest<PagedResult<TeamMemberResponse>>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }
}
