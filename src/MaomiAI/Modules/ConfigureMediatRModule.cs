// <copyright file="ConfigureMediatRModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra;

namespace MaomiAI.Modules;

/// <summary>
/// 配置 MediatR .
/// </summary>
public class ConfigureMediatRModule : IModule
{
    private readonly IConfiguration _configuration;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureMediatRModule"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    public ConfigureMediatRModule(IConfiguration configuration)
    {
        _configuration = configuration;
        _systemOptions = configuration.Get<SystemOptions>()!;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        context.Services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(context.Modules.Select(x => x.Assembly).Distinct().ToArray());
            options.RegisterGenericHandlers = true;
        });
    }
}