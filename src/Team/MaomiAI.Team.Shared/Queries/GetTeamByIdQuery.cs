// <copyright file="GetTeamByIdQuery.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Team.Shared.Models;
using MediatR;

namespace MaomiAI.Team.Shared.Queries
{
    /// <summary>
    /// 根据ID获取团队查询.
    /// </summary>
    public class GetTeamByIdQuery : IRequest<TeamDto?>
    {
        /// <summary>
        /// 团队ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTeamByIdQuery"/> class.
        /// </summary>
        /// <param name="id">团队ID.</param>
        public GetTeamByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}