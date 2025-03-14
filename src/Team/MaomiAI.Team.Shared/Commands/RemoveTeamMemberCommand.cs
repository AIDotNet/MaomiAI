// <copyright file="RemoveTeamMemberCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;

using MediatR;

namespace MaomiAI.Team.Shared.Commands;

/// <summary>
/// 移除团队成员命令.
/// </summary>
public class RemoveTeamMemberCommand : IRequest
{
    /// <summary>
    /// 要移除的团队成员ID.
    /// </summary>
    [Required]
    public int MemberId { get; set; }
}