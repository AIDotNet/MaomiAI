// <copyright file="DatabaseCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using MaomiAI.Database.Postgres;

namespace MaomiAI.Database;

/// <summary>
/// DatabaseCoreModule.
/// </summary>
[InjectModule<DatabasePostgresModule>]
public class DatabaseCoreModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
    }
}
