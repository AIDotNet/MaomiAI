// <copyright file="CreateTeamCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;

using MediatR;

namespace MaomiAI.Team.Shared.Commands;

/// <summary>
/// 创建团队命令.
/// </summary>
public class CreateTeamCommand : IRequest<Guid>
{
    /// <summary>
    /// 团队名称.
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 团队描述.
    /// </summary>
    [Required]
    [StringLength(500)]
    public string Description { get; set; } = null!;

    /// <summary>
    /// 团队头像URL.
    /// </summary>
    [StringLength(500)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 状态：true-正常，false-禁用.
    /// </summary>
    public bool Status { get; set; } = true;
}