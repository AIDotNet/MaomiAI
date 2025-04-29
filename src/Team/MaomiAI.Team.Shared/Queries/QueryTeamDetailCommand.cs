// <copyright file="QueryTeamDetailCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;

namespace MaomiAI.Team.Shared.Queries;

/// <summary>
/// 获取团队详细信息.
/// </summary>
public class QueryTeamDetailCommand : IRequest<TeamDetailResponse>
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    public Guid TeamId { get; set; }
}