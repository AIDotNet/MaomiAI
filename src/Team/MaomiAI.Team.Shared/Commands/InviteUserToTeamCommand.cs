// <copyright file="InviteUserToTeamCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MaomiAI.Team.Shared.Commands;

/// <summary>
/// 邀请用户加入团队命令.
/// </summary>
public class InviteUserToTeamCommand : IRequest
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    [Required]
    public Guid TeamId { get; set; }

    /// <summary>
    /// 被邀请的用户ID.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
}