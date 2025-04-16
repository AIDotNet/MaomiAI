// <copyright file="UpdateTeamMemberRoleCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MaomiAI.Team.Shared.Commands;

/// <summary>
/// 更新团队成员角色命令.
/// </summary>
public class UpdateTeamMemberRoleCommand : IRequest
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    [Required]
    public Guid TeamId { get; set; }

    /// <summary>
    /// 用户ID.
    /// </summary>
    [Required]
    public Guid MemberUserId { get; set; }

    /// <summary>
    /// 是否为管理员.
    /// </summary>
    public bool IsAdmin { get; set; }
}