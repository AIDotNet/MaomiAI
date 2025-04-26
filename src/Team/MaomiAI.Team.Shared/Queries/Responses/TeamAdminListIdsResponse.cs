// <copyright file="CheckTeamAdminQuery.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MaomiAI.Team.Shared.Queries.Responses;

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
