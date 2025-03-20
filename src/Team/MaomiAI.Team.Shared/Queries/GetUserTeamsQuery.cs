// <copyright file="GetUserTeamsQuery.cs" company="MaomiAI">
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
    /// 获取用户所属的所有团队查询.
    /// </summary>
    public class GetUserTeamsQuery : IRequest<List<TeamDto>>
    {
        private string? _keyword;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserTeamsQuery"/> class.
        /// </summary>
        /// <param name="userId">用户ID.</param>
        public GetUserTeamsQuery(Guid userId)
        {
            UserId = userId;
        }

        /// <summary>
        /// 用户ID.
        /// </summary>
        [Required]
        public Guid UserId { get; }

        /// <summary>
        /// 关键词（团队名称）.
        /// </summary>
        public string? Keyword
        {
            get => _keyword;
            set => _keyword = !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;
        }

        /// <summary>
        /// 是否只返回用户是管理员的团队.
        /// </summary>
        public bool? IsAdmin { get; set; }

        /// <summary>
        /// 是否只返回用户是所有者的团队.
        /// </summary>
        public bool? IsOwner { get; set; }
    }
}