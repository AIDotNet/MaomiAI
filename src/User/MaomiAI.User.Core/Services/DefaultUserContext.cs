// <copyright file="DefaultUserContext.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;

using MaomiAI.Infra.Models;

namespace MaomiAI.User.Core.Services
{
    /// <summary>
    /// 默认用户上下文实现.
    /// <para>
    /// DefaultUserContext 类是 UserContext 接口的默认实现，用于在应用程序中存储和管理当前用户的信息。
    /// 它采用充血模型设计，提供了一系列链式调用方法来设置和修改用户信息，使得代码更加简洁和可读。
    /// </para>
    /// <para>
    /// 主要功能包括：
    /// - 存储用户基本信息（ID、用户名、昵称、邮箱、头像等）
    /// - 管理用户认证状态
    /// - 维护用户角色列表
    /// - 提供角色检查方法（HasRole、HasAnyRole、HasAllRoles）
    /// - 支持在每次请求开始时清除用户信息
    /// </para>
    /// <para>
    /// 该类通常与 AuthMiddleware 配合使用，由中间件负责从 JWT 令牌中提取用户信息并填充到 UserContext 中，
    /// 然后在控制器和服务中通过依赖注入获取当前用户信息。
    /// </para>
    /// <para>
    /// 注意：该类的修改方法（SetUserInfo、SetAuthenticated、AddRole 等）被标记为 internal，
    /// 只能在当前程序集内部使用，这样可以防止外部代码随意修改用户信息，提高安全性。
    /// </para>
    /// </summary>
    [InjectOnScoped]
    public class DefaultUserContext : UserContext
    {
        private bool _isAuthenticated;
        private Guid _userId;
        private string _userName = string.Empty;
        private string _nickName = string.Empty;
        private string _email = string.Empty;
        private string _avatarUrl = string.Empty;
        private readonly List<string> _roles = new();

        /// <summary>
        /// 获取用户ID.
        /// </summary>
        public Guid UserId => _userId;

        /// <summary>
        /// 获取用户名称.
        /// </summary>
        public string UserName => _userName;

        /// <summary>
        /// 获取用户昵称.
        /// </summary>
        public string NickName => _nickName;

        /// <summary>
        /// 获取用户邮箱.
        /// </summary>
        public string Email => _email;

        /// <summary>
        /// 获取用户头像URL.
        /// </summary>
        public string AvatarUrl => _avatarUrl;

        /// <summary>
        /// 获取用户角色列表.
        /// </summary>
        public IReadOnlyList<string> Roles => _roles.AsReadOnly();

        /// <summary>
        /// 设置用户基本信息.
        /// </summary>
        /// <param name="userId">用户ID.</param>
        /// <param name="userName">用户名.</param>
        /// <param name="nickName">昵称.</param>
        /// <param name="email">邮箱.</param>
        /// <param name="avatarUrl">头像URL.</param>
        /// <returns>当前用户上下文实例，支持链式调用.</returns>
        internal DefaultUserContext SetUserInfo(Guid userId, string userName, string nickName, string email, string avatarUrl)
        {
            _userId = userId;
            _userName = userName ?? string.Empty;
            _nickName = nickName ?? string.Empty;
            _email = email ?? string.Empty;
            _avatarUrl = avatarUrl ?? string.Empty;
            return this;
        }

        /// <summary>
        /// 设置用户认证状态.
        /// </summary>
        /// <param name="isAuthenticated">是否已认证.</param>
        /// <returns>当前用户上下文实例，支持链式调用.</returns>
        internal DefaultUserContext SetAuthenticated(bool isAuthenticated)
        {
            _isAuthenticated = isAuthenticated;
            return this;
        }

        /// <summary>
        /// 添加用户角色.
        /// </summary>
        /// <param name="role">角色名称.</param>
        /// <returns>当前用户上下文实例，支持链式调用.</returns>
        internal DefaultUserContext AddRole(string role)
        {
            if (!string.IsNullOrEmpty(role) && !_roles.Contains(role))
            {
                _roles.Add(role);
            }
            return this;
        }

        /// <summary>
        /// 添加多个用户角色.
        /// </summary>
        /// <param name="roles">角色列表.</param>
        /// <returns>当前用户上下文实例，支持链式调用.</returns>
        internal DefaultUserContext AddRoles(IEnumerable<string> roles)
        {
            if (roles == null)
            {
                return this;
            }

            foreach (var role in roles)
            {
                AddRole(role);
            }
            return this;
        }

        /// <summary>
        /// 清除用户信息.
        /// </summary>
        /// <returns>当前用户上下文实例，支持链式调用.</returns>
        internal DefaultUserContext Clear()
        {
            _isAuthenticated = false;
            _userId = Guid.Empty;
            _userName = string.Empty;
            _nickName = string.Empty;
            _email = string.Empty;
            _avatarUrl = string.Empty;
            _roles.Clear();
            return this;
        }

        /// <summary>
        /// 检查当前用户是否已认证.
        /// </summary>
        /// <returns>如果用户已认证则返回true，否则返回false.</returns>
        public bool IsAuthenticated() => _isAuthenticated;

        /// <summary>
        /// 检查当前用户是否具有指定角色.
        /// </summary>
        /// <param name="role">角色名称.</param>
        /// <returns>如果用户具有指定角色则返回true，否则返回false.</returns>
        public bool HasRole(string role) => !string.IsNullOrEmpty(role) && _roles.Contains(role);

        /// <summary>
        /// 检查当前用户是否具有指定角色之一.
        /// </summary>
        /// <param name="roles">角色列表.</param>
        /// <returns>如果用户具有指定角色之一则返回true，否则返回false.</returns>
        public bool HasAnyRole(params string[] roles)
        {
            if (roles == null || roles.Length == 0)
            {
                return false;
            }

            return roles.Any(role => HasRole(role));
        }

        /// <summary>
        /// 检查当前用户是否具有所有指定角色.
        /// </summary>
        /// <param name="roles">角色列表.</param>
        /// <returns>如果用户具有所有指定角色则返回true，否则返回false.</returns>
        public bool HasAllRoles(params string[] roles)
        {
            if (roles == null || roles.Length == 0)
            {
                return true;
            }

            return roles.All(role => HasRole(role));
        }
    }
}
