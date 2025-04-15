// <copyright file="CheckTeamNameCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Team.Shared.Commands;

/// <summary>
/// 检查团队名称是否存在.
/// </summary>
public class CheckTeamNameCommand
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// 团队名称.
    /// </summary>
    public string Name { get; set; } = null!;
}