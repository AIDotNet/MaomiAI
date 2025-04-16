// <copyright file="RevokeAdminPermissionCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Team.Shared.Commands;

/// <summary>
/// 撤销团队成员管理员权限命令.
/// </summary>
public class SetTeamMemberPermissionCommand : IRequest<EmptyDto>
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    public Guid TeamId { get; set; }

    /// <summary>
    /// 用户ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 启用.
    /// </summary>
    public bool IsEnable { get; set; } = true;

    /// <summary>
    /// 设置为管理员.
    /// </summary>
    public bool? IsAdmin { get; set; }
}