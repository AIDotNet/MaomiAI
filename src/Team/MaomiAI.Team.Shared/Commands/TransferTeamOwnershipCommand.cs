// <copyright file="TransferTeamOwnershipCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MaomiAI.Team.Shared.Commands
{
    /// <summary>
    /// 转移团队所有权命令.
    /// </summary>
    public class TransferTeamOwnershipCommand : IRequest
    {
        /// <summary>
        /// 团队ID.
        /// </summary>
        [Required]
        public Guid TeamId { get; set; }

        /// <summary>
        /// 新所有者用户ID.
        /// </summary>
        [Required]
        public Guid NewOwnerUserId { get; set; }
    }
}