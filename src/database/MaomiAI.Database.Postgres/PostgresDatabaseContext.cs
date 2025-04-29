// <copyright file="PostgresDatabaseContext.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Database.Postgres;

/// <summary>
/// PostgresDatabaseContext.
/// </summary>
public class PostgresDatabaseContext : DatabaseContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostgresDatabaseContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="contextOptions"></param>
    public PostgresDatabaseContext(DbContextOptions options, IServiceProvider serviceProvider, DatabaseOptions contextOptions)
        : base(options, serviceProvider, contextOptions)
    {
    }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // postgres 需要开启此扩展，以便支持 uuid_generate_v4()
        modelBuilder.HasPostgresExtension("uuid-ossp");

        base.OnModelCreating(modelBuilder);
    }
}
