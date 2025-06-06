// <copyright file="QueryTeamSimpleCommandResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Team.Shared.Queries.Responses;

/// <summary>
/// 团队基础信息.
/// </summary>
public class QueryTeamSimpleCommandResponse : AuditsInfo
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 是否所有者.
    /// </summary>
    public bool IsRoot { get; init; }

    /// <summary>
    /// 是否管理员.
    /// </summary>
    public bool IsAdmin { get; init; }

    /// <summary>
    /// 团队名称.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// 团队描述.
    /// </summary>
    public string Description { get; set; } = default!;

    /// <summary>
    /// 团队头像路径.
    /// </summary>
    public string AvatarUrl { get; set; } = default!;

    /// <summary>
    /// 团队已被禁用.
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
    public string OwnUserName { get; set; } = default!;
}
