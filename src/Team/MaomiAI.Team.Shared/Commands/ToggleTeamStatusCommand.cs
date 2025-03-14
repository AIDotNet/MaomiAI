// <copyright file="ToggleTeamStatusCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;

using MediatR;

namespace MaomiAI.Team.Shared.Commands;

/// <summary>
/// 切换团队状态命令.
/// </summary>
public class ToggleTeamStatusCommand : IRequest
{
    /// <summary>
    /// 团队ID列表.
    /// </summary>
    [Required]
    public List<Guid> TeamIds { get; set; } = new();

    /// <summary>
    /// 状态：true-正常，false-禁用.
    /// </summary>
    public bool Status { get; set; }
}