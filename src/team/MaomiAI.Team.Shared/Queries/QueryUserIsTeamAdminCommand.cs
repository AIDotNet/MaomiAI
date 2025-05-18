// <copyright file="QueryUserIsTeamAdminCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;

namespace MaomiAI.Team.Shared.Queries;

/// <summary>
/// 该用户是否是团队管理员.
/// </summary>
public class QueryUserIsTeamAdminCommand : IRequest<QueryUserIsTeamAdminCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 用户 id.
    /// </summary>
    public Guid UserId { get; init; }
}
