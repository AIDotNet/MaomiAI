// <copyright file="GetTeamMemberByIdQuery.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using MaomiAI.Team.Shared.Models;
using MediatR;

namespace MaomiAI.Team.Shared.Queries
{
    /// <summary>
    /// 根据ID获取团队成员查询.
    /// </summary>
    public class GetTeamMemberByIdQuery : IRequest<TeamMemberDto?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTeamMemberByIdQuery"/> class.
        /// </summary>
        /// <param name="teamId">团队ID.</param>
        /// <param name="userId">用户ID.</param>
        public GetTeamMemberByIdQuery(Guid teamId, Guid userId)
        {
            TeamId = teamId;
            UserId = userId;
        }

        /// <summary>
        /// 团队ID.
        /// </summary>
        [Required]
        public Guid TeamId { get; }

        /// <summary>
        /// 用户ID.
        /// </summary>
        [Required]
        public Guid UserId { get; }
    }
}