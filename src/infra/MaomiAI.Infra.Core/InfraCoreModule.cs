﻿// <copyright file="InfraCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;

namespace MaomiAI.Infra;

/// <summary>
/// InfraCoreModule.
/// </summary>
[InjectModule<InfraConfigurationModule>]
public class InfraCoreModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
    }
}
