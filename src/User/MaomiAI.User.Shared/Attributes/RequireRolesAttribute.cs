// <copyright file="RequireRolesAttribute.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.User.Shared.Attributes;

/// <summary>
/// 要求用户具有指定角色的授权特性.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequireRolesAttribute : AuthorizeAttribute
{
    /// <summary>
    /// 角色检查模式.
    /// </summary>
    public enum RoleCheckMode
    {
        /// <summary>
        /// 用户必须具有所有指定角色.
        /// </summary>
        All,

        /// <summary>
        /// 用户只需具有任一指定角色.
        /// </summary>
        Any
    }

    /// <summary>
    /// 获取或设置角色检查模式.
    /// </summary>
    public RoleCheckMode Mode { get; set; } = RoleCheckMode.Any;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequireRolesAttribute"/> class.
    /// </summary>
    /// <param name="roles">所需角色，以逗号分隔.</param>
    public RequireRolesAttribute(string roles)
        : base()
    {
        Roles = roles;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequireRolesAttribute"/> class.
    /// </summary>
    /// <param name="mode">角色检查模式.</param>
    /// <param name="roles">所需角色，以逗号分隔.</param>
    public RequireRolesAttribute(RoleCheckMode mode, string roles)
        : base()
    {
        Mode = mode;
        Roles = roles;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequireRolesAttribute"/> class.
    /// </summary>
    /// <param name="roles">所需角色列表.</param>
    public RequireRolesAttribute(params string[] roles)
        : base()
    {
        Roles = string.Join(",", roles);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequireRolesAttribute"/> class.
    /// </summary>
    /// <param name="mode">角色检查模式.</param>
    /// <param name="roles">所需角色列表.</param>
    public RequireRolesAttribute(RoleCheckMode mode, params string[] roles)
        : base()
    {
        Mode = mode;
        Roles = string.Join(",", roles);
    }
}