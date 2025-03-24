// <copyright file="UserCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using MaomiAI.User.Api;
using MaomiAI.User.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.User.Core
{
    /// <summary>
    /// 用户模块.
    /// </summary>
    [InjectModule<UserApiModule>]
    public class UserCoreModule : IModule
    {
        /// <inheritdoc/>
        public void ConfigureServices(ServiceContext context)
        {
            // 注册认证中间件
            context.Services.AddScoped<AuthMiddleware>();
        }
    }
}