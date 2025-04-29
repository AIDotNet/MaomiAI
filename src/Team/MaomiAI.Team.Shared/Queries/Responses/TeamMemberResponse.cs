// <copyright file="TeamMemberResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Team.Shared.Queries.Responses;

public class TeamMemberResponse : AuditsInfo
{
    /// <summary>
    /// 成员ID.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// 用户名称.
    /// </summary>
    public string UserName { get; init; }

    /// <summary>
    /// 昵称.
    /// </summary>
    public string NickName { get; set; } = null!;

    /// <summary>
    /// 用户头像.
    /// </summary>
    public string UserAvatarPath { get; init; }

    /// <summary>
    /// 是否为管理员.
    /// </summary>
    public bool IsAdmin { get; init; }

    /// <summary>
    /// 团队所有者.
    /// </summary>
    public bool IsOwner { get; init; }
}
