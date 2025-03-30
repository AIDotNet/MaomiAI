﻿// <copyright file="MainModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using Maomi.I18n;
using MaomiAI.Database;
using MaomiAI.Filters;
using MaomiAI.Infra;
using MaomiAI.Modules;
using MaomiAI.Store;
using MaomiAI.Team.Core;
using MaomiAI.User.Core;

namespace MaomiAI
{
    /// <summary>
    /// MainModule.
    /// </summary>
    [InjectModule<InfraCoreModule>]
    [InjectModule<DatabaseCoreModule>]
    [InjectModule<EmbeddingCoreModule>]
    [InjectModule<DocumentModule>]
    [InjectModule<StoreCoreModule>]
    [InjectModule<UserCoreModule>]
    [InjectModule<TeamCoreModule>]
    [InjectModule<ApiModule>]
    public partial class MainModule : IModule
    {
        private readonly IConfiguration _configuration;
        private readonly SystemOptions _systemOptions;

        public MainModule(IConfiguration configuration)
        {
            _configuration = configuration;
            _systemOptions = configuration.Get<SystemOptions>()!;
        }

        /// <inheritdoc/>
        public void ConfigureServices(ServiceContext context)
        {
            context.Services.AddExceptionHandler<CustomGlobalExceptionHandler>();
            context.Services.AddI18nAspNetCore();
        }
    }
}