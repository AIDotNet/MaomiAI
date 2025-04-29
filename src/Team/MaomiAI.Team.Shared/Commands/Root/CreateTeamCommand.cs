// <copyright file="CreateTeamCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Team.Shared.Commands.Root;

/// <summary>
/// 创建团队命令.
/// </summary>
public class CreateTeamCommand : IRequest<GuidResponse>
{
    /// <summary>
    /// 团队名称.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 团队描述.
    /// </summary>
    public string Description { get; set; } = null!;
}
