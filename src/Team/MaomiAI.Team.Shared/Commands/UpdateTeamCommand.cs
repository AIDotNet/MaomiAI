// <copyright file="UpdateTeamCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MaomiAI.Team.Shared.Commands
{
    /// <summary>
    /// 更新团队命令.
    /// </summary>
    public class UpdateTeamCommand : IRequest
    {
        /// <summary>
        /// 团队ID.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 团队名称.
        /// </summary>
        [StringLength(50, MinimumLength = 2)]
        public string? Name { get; set; }

        /// <summary>
        /// 团队描述.
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// 团队头像URL.
        /// </summary>
        [StringLength(500)]
        public string? Avatar { get; set; }
    }
}