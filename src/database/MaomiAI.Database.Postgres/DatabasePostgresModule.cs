﻿using Maomi;
using MaomiAI.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Database.Postgres;

/// <summary>
/// DatabasePostgresModule.
/// </summary>
public class DatabasePostgresModule : IModule
{
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabasePostgresModule"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="loggerFactory"></param>
    public DatabasePostgresModule(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        _loggerFactory = loggerFactory;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        using var ioc = context.Services.BuildServiceProvider();
        var systemOptions = ioc.GetRequiredService<SystemOptions>();

        if (!"postgres".Equals(systemOptions.DBType, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var dbContextOptions = new DBContextOptions
        {
            ConfigurationAssembly = typeof(DatabasePostgresModule).Assembly,
            EntityAssembly = typeof(MaomiAI.Database.MaomiaiContext).Assembly
        };

        context.Services.AddSingleton(dbContextOptions);

        // 配置兼容 Datetime
        // todo: 应该可以去掉
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        Action<DbContextOptionsBuilder> contextOptionsBuilder = o =>
        {
            o.UseNpgsql(systemOptions.Database)
            .ConfigureWarnings(
                b => b.Ignore([
                    CoreEventId.ServiceProviderCreated,
                        CoreEventId.ContextInitialized,
                        CoreEventId.ContextDisposed,
                        CoreEventId.LazyLoadOnDisposedContextWarning,
                        CoreEventId.QueryCompilationStarting,
                        CoreEventId.StateChanged,
                        CoreEventId.SaveChangesCanceled,
                        CoreEventId.SaveChangesCompleted,
                        CoreEventId.SensitiveDataLoggingEnabledWarning,
                        CoreEventId.QueryExecutionPlanned,
                        CoreEventId.StartedTracking,
                        RelationalEventId.ConnectionOpening,
                        RelationalEventId.ConnectionCreating,
                        RelationalEventId.ConnectionCreated,
                        RelationalEventId.ConnectionClosing,
                        RelationalEventId.ConnectionClosed,
                        RelationalEventId.DataReaderClosing,
                        RelationalEventId.DataReaderDisposing,
                        RelationalEventId.CommandCanceled,
                        RelationalEventId.CommandCreated,
                        RelationalEventId.CommandCreating,
                        RelationalEventId.CommandInitialized,
                        RelationalEventId.BoolWithDefaultWarning,
                        RelationalEventId.ModelValidationKeyDefaultValueWarning,
                ]))
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
        };

        context.Services.AddDbContext<MaomiaiContext>(contextOptionsBuilder);

        DbContextOptionsBuilder<MaomiaiContext> options = new();
        contextOptionsBuilder.Invoke(options);

        using var dbContext = new MaomiaiContext(options.Options, ioc, dbContextOptions);

        // 如果数据库不存在，则会创建数据库及其所有表。
        dbContext.Database.EnsureCreated();
    }
}