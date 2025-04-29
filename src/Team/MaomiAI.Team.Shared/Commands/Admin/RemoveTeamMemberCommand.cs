// <copyright file="RemoveTeamMemberCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Team.Shared.Commands.Admin;

/// <summary>
/// 移除团队成员命令.
/// </summary>
public class RemoveTeamMemberCommand : IRequest<EmptyCommandResponse>
{
    public Guid TeamId { get; set; }

    /// <summary>
    /// 要移除的团队成员ID.
    /// </summary>
    public Guid UserId { get; set; }
}