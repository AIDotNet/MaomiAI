// <copyright file="QueryTeamSimpleCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Team.Shared.Queries.Responses;
using MediatR;

namespace MaomiAI.Team.Shared.Queries;

/// <summary>
/// 获取团队简单信息.
/// </summary>
public class QueryTeamSimpleCommand : IRequest<TeamSimpleResponse>
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    public Guid TeamId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamSimpleCommand"/> class.
    /// </summary>
    /// <param name="id">团队ID.</param>
    public QueryTeamSimpleCommand(Guid id)
    {
        TeamId = id;
    }
}