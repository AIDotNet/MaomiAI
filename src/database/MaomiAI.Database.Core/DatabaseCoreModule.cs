// <copyright file="DatabaseCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using MaomiAI;
using MaomiAI.Database.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MaomiAI.Database
{
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

            // 如果使用内存数据库
            if ("inmemory".Equals(systemOptions.DBType, StringComparison.OrdinalIgnoreCase))
            {
                DatabaseOptions? dbContextOptions = new()
                {
                    ConfigurationAssembly = typeof(DatabaseCoreModule).Assembly,
                    EntityAssembly = typeof(MaomiaiContext).Assembly
                };

                context.Services.AddSingleton(dbContextOptions);

                // 注册内存数据库
                context.Services.AddDbContext<MaomiaiContext>(options =>
                {
                    options.UseInMemoryDatabase(systemOptions.Database);
                });

                // 创建数据库
                using ServiceProvider? serviceProvider = context.Services.BuildServiceProvider();
                using IServiceScope? scope = serviceProvider.CreateScope();
                MaomiaiContext? dbContext = scope.ServiceProvider.GetRequiredService<MaomiaiContext>();
                dbContext.Database.EnsureCreated();
            }
        }
    }
}