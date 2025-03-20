// <copyright file="UserContext.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra.Models
{
    /// <summary>
    /// 用户上下文接口，提供当前用户的信息.
    /// </summary>
    public interface UserContext
    {
        /// <summary>
        /// 用户ID.
        /// </summary>
        Guid UserId { get; }

        /// <summary>
        /// 用户名称.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 用户昵称.
        /// </summary>
        string NickName { get; }

        /// <summary>
        /// 用户邮箱.
        /// </summary>
        string Email { get; }

        /// <summary>
        /// 用户头像URL.
        /// </summary>
        string AvatarUrl { get; }

        /// <summary>
        /// 用户角色列表.
        /// </summary>
        IReadOnlyList<string> Roles { get; }

        /// <summary>
        /// 检查当前用户是否已认证.
        /// </summary>
        /// <returns>如果用户已认证则返回true，否则返回false.</returns>
        bool IsAuthenticated();

        /// <summary>
        /// 检查当前用户是否具有指定角色.
        /// </summary>
        /// <param name="role">角色名称.</param>
        /// <returns>如果用户具有指定角色则返回true，否则返回false.</returns>
        bool HasRole(string role);

        /// <summary>
        /// 检查当前用户是否具有指定角色之一.
        /// </summary>
        /// <param name="roles">角色列表.</param>
        /// <returns>如果用户具有指定角色之一则返回true，否则返回false.</returns>
        bool HasAnyRole(params string[] roles);

        /// <summary>
        /// 检查当前用户是否具有所有指定角色.
        /// </summary>
        /// <param name="roles">角色列表.</param>
        /// <returns>如果用户具有所有指定角色则返回true，否则返回false.</returns>
        bool HasAllRoles(params string[] roles);
    }
}