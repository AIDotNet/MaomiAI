﻿// <copyright file="MainModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.I18n;
using MaomiAI.AiModel.Core;
using MaomiAI.Database;
using MaomiAI.Filters;
using MaomiAI.Infra;
using MaomiAI.Modules;
using MaomiAI.Note;
using MaomiAI.Plugin.Core;
using MaomiAI.Prompt.Core;
using MaomiAI.Public;
using MaomiAI.Store;
using MaomiAI.Team.Core;
using MaomiAI.User.Core;

namespace MaomiAI;

/// <summary>
/// MainModule.
/// </summary>
[InjectModule<InfraCoreModule>]
[InjectModule<DatabaseCoreModule>]
[InjectModule<DocumentModule>]
[InjectModule<StoreCoreModule>]
[InjectModule<UserCoreModule>]
[InjectModule<TeamCoreModule>]
[InjectModule<AiModelCoreModule>]
[InjectModule<PublicCoreModule>]
[InjectModule<PromptCoreModule>]
[InjectModule<PluginCoreModule>]
[InjectModule<NoteCoreModule>]
[InjectModule<ApiModule>]
public partial class MainModule : IModule
{
    private readonly IConfiguration _configuration;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainModule"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    public MainModule(IConfiguration configuration)
    {
        _configuration = configuration;
        _systemOptions = configuration.Get<SystemOptions>()!;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        // 添加HTTP上下文访问器
        context.Services.AddHttpContextAccessor();
        context.Services.AddExceptionHandler<MaomiExceptionHandler>();
        context.Services.AddI18nAspNetCore();
    }
}