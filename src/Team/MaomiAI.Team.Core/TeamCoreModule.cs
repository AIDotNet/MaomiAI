// <copyright file="TeamApiModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Team.Api;

namespace MaomiAI.Team.Core
{
    /// <summary>
    /// 团队API模块.
    /// </summary>
    [InjectModule<TeamApiModule>]
    public class TeamCoreModule : IModule
    {
        /// <summary>
        /// 配置服务.
        /// </summary>
        /// <param name="context">服务上下文.</param>
        public void ConfigureServices(ServiceContext context)
        {
        }
    }
}