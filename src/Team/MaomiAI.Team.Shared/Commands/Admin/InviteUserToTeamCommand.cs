// <copyright file="InviteUserToTeamCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MaomiAI.Team.Shared.Commands.Admin;

/// <summary>
/// 邀请用户加入团队命令.
/// </summary>
public class InviteUserToTeamCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    [Required]
    public Guid TeamId { get; init; }

    /// <summary>
    /// 被邀请的用户ID.
    /// </summary>
    [Required]
    public Guid? UserId { get; init; }

    /// <summary>
    /// 用户名.
    /// </summary>
    public string? UserName { get; init; } = default!;
}