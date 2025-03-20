// <copyright file="TeamDto.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Team.Shared.Models
{
    /// <summary>
    /// 团队数据传输对象.
    /// </summary>
    public class TeamDto
    {
        /// <summary>
        /// 团队ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 团队名称.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 团队描述.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// 团队头像.
        /// </summary>
        public string Avatar { get; set; } = null!;

        /// <summary>
        /// 状态：true-正常，false-禁用.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// 更新时间.
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }

        /// <summary>
        /// 创建人ID.
        /// </summary>
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 当前用户是否是团队所有者（仅在查询用户所属团队时有值）.
        /// </summary>
        public bool IsOwner { get; set; }

        /// <summary>
        /// 当前用户是否是团队管理员（仅在查询用户所属团队时有值）.
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}