// <copyright file="TeamMemberDto.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Team.Shared.Models
{
    /// <summary>
    /// 团队成员数据传输对象.
    /// </summary>
    public class TeamMemberDto
    {
        /// <summary>
        /// 成员ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 团队ID.
        /// </summary>
        public Guid TeamId { get; set; }

        /// <summary>
        /// 用户ID.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户名称.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 用户头像.
        /// </summary>
        public string? UserAvatar { get; set; }

        /// <summary>
        /// 是否为团队所有者.
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// 是否为管理员.
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 加入时间.
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
    }
}