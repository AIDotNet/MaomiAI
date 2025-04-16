// <copyright file="UpdateTeamCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;

namespace MaomiAI.Team.Shared.Commands;

/// <summary>
/// 更新团队命令.
/// </summary>
public class UpdateTeamCommand : IRequest
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// 团队名称.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 团队描述.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 团队头像 id.
    /// </summary>
    public Guid AvatarFileId { get; set; }

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
    public string? Markdown { get; set; } = null!;
}
