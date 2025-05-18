// <copyright file="UpdateTeamInfoCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MaomiAI.Team.Shared.Commands.Root;

/// <summary>
/// 更新团队信息命令.
/// </summary>
public class UpdateTeamInfoCommand : IRequest
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    [Required]
    public Guid TeamId { get; set; }

    /// <summary>
    /// 团队名称.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 团队描述.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// 禁用团队.
    /// </summary>
    public bool IsDisable { get; set; }

    /// <summary>
    /// 是否公开,能够被外部搜索.
    /// </summary>
    public bool IsPublic { get; set; }

    /// <summary>
    /// 团队详细介绍.
    /// </summary>
    public string Markdown { get; set; } = default!;
}
