// <copyright file="QueryUserJoinedTeamCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;

namespace MaomiAI.Team.Shared.Queries.User;

/// <summary>
/// 分页查询用户已加入的团队列表.
/// </summary>
public class QueryUserJoinedTeamCommand : PagedParamter, IRequest<PagedResult<TeamSimpleResponse>>
{
    /// <summary>
    /// 查询关键字.
    /// </summary>
    public string? Keyword { get; set; }
}