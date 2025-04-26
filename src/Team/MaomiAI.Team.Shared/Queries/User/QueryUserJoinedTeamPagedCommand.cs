// <copyright file="QueryUserJoinedTeamPagedCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Models;
using MediatR;

namespace MaomiAI.Team.Shared.Queries.User;

/// <summary>
/// 分页查询用户已加入的团队列表.
/// </summary>
public class QueryUserJoinedTeamPagedCommand : PagedParamter, IRequest<PagedResult<UserJoinedTeamItemResponse>>
{
    /// <summary>
    /// 查询关键字.
    /// </summary>
    public string? Keyword { get; set; }
}