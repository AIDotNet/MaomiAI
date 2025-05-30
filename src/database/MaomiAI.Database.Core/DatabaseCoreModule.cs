﻿// <copyright file="DatabaseCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database.Commands;
using MaomiAI.Database.Postgres;
using MaomiAI.Database.Queries;
using MaomiAI.Infra;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace MaomiAI.Database;

/// <summary>
/// DatabaseCoreModule.
/// </summary>
[InjectModule<DatabasePostgresModule>]
public class DatabaseCoreModule : IModule
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseCoreModule"/> class.
    /// </summary>
    /// <param name="configuration">配置.</param>
    public DatabaseCoreModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        SystemOptions? systemOptions = _configuration.Get<SystemOptions>();
        if (systemOptions == null)
        {
            return;
        }

        // 添加 redis
        AddStackExchangeRedis(context.Services, new RedisConfiguration
        {
            ConnectionString = systemOptions.Redis,
            PoolSize = 10,
            KeyPrefix = "maomi:",
            ConnectTimeout = 5000,
            IsDefault = true
        });

        // 如果使用内存数据库
        if ("inmemory".Equals(systemOptions.DBType, StringComparison.OrdinalIgnoreCase))
        {
            DatabaseOptions? dbContextOptions = new()
            {
                ConfigurationAssembly = typeof(DatabaseCoreModule).Assembly,
                EntityAssembly = typeof(DatabaseContext).Assembly
            };

            context.Services.AddSingleton(dbContextOptions);

            // 注册内存数据库
            context.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseInMemoryDatabase(systemOptions.Database);
            });

            // 创建数据库
            using ServiceProvider? serviceProvider = context.Services.BuildServiceProvider();
            using IServiceScope? scope = serviceProvider.CreateScope();
            DatabaseContext? dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            dbContext.Database.EnsureCreated();
        }
    }

    private static void AddStackExchangeRedis(IServiceCollection services, RedisConfiguration redisConfiguration)
    {
        services.AddSingleton<ISerializer, SystemTextJsonSerializer>();

        services.AddSingleton<IRedisClientFactory, RedisClientFactory>();

        services.AddSingleton((provider) => provider
            .GetRequiredService<IRedisClientFactory>()
            .GetDefaultRedisClient()
            .GetDefaultDatabase());

        services.AddSingleton(redisConfiguration);
    }
}