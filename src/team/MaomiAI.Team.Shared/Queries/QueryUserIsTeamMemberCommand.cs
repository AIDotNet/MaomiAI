// <copyright file="QueryUserIsTeamAdminCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;

namespace MaomiAI.Team.Shared.Queries;

/// <summary>
/// 查询用户是否团队成员.
/// </summary>
public class QueryUserIsTeamMemberCommand : IRequest<QueryUserIsTeamMemberCommandResponse>
{
    public Guid TeamId { get; init; }
    public Guid UserId { get; init; }
}
