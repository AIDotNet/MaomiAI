// <copyright file="GetTeamMembersQuery.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using MaomiAI.Team.Shared.Models;
using MaomiAI.User.Shared.Models;
using MediatR;

namespace MaomiAI.Team.Shared.Queries
{
    /// <summary>
    /// 获取团队成员列表查询.
    /// </summary>
    public class GetTeamMembersQuery : IRequest<PagedResult<TeamMemberDto>>
    {
        private int _page = 1;
        private int _pageSize = 20;
        private string? _keyword;

        /// <summary>
        /// 团队ID.
        /// </summary>
        [Required]
        public Guid TeamId { get; set; }

        /// <summary>
        /// 页码，从1开始.
        /// </summary>
        public int Page
        {
            get => _page;
            set => _page = Math.Max(1, value);
        }

        /// <summary>
        /// 每页大小.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Clamp(value, 1, 100); // 限制每页最大数量为100
        }

        /// <summary>
        /// 关键词（用户名）.
        /// </summary>
        public string? Keyword
        {
            get => _keyword;
            set => _keyword = !string.IsNullOrWhiteSpace(value) ? value.Trim() : null;
        }

        /// <summary>
        /// 角色过滤（管理员/普通成员）.
        /// </summary>
        public bool? IsAdmin { get; set; }
    }
}