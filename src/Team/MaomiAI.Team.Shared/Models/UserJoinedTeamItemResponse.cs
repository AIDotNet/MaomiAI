// <copyright file="TeamDto.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;

namespace MaomiAI.Team.Shared.Models;

/// <summary>
/// 用户已加入的团队.
/// </summary>
public class UserJoinedTeamItemResponse : AuditsInfo
{
    /// <summary>
    /// 团队ID.
    /// </summary>
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
    /// 团队头像.
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
    /// 所有者 id.
    /// </summary>
    public Guid OwnUserId { get; set; }

    /// <summary>
    /// 所有人名字.
    /// </summary>
    public string OwnUserName { get; set; } = null!;
}
