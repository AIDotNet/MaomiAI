// <copyright file="DeleteTeamCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MaomiAI.Team.Shared.Commands.Root;

/// <summary>
/// 删除团队命令.
/// </summary>
public class DeleteTeamCommand : IRequest
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    public Guid Id { get; set; }
}