// <copyright file="InfraCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.MQ;
using MaomiAI.Infra.Defaults;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Infra
{
    /// <summary>
    /// InfraCoreModule.
    /// </summary>
    [InjectModule<InfraConfigurationModule>]
    public class InfraCoreModule : IModule
    {
        /// <inheritdoc/>
        public void ConfigureServices(ServiceContext context)
        {
            context.Services.AddSingleton<IIdProvider>(new DefaultIdProvider(0));
            context.Services.AddHttpContextAccessor();
            context.Services.AddScoped<UserContext, DefaultUserContext>();
        }
    }
}