// <copyright file="PluginCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using MaomiAI.Plugin.Api;

namespace MaomiAI.Plugin.Core;

/// <summary>
/// Module for the Plugin Core.
/// </summary>
[InjectModule<PluginApiModule>]
public class PluginCoreModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
    }
}
