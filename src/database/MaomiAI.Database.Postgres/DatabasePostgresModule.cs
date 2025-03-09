using Maomi;
using MaomiAI.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace MaomiAI.Database.Postgres;

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

    public void ConfigureServices(ServiceContext context)
    {
        var ioc = context.Services.BuildServiceProvider();
        var systemOptions = ioc.GetRequiredService<SystemOptions>();

        if (!"postgres".Equals(systemOptions.DBType, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        // 配置兼容 Datetime
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

        context.Services.AddDbContext<MaomiaiContext>(o =>
        {
            // 注入服务以及配置要忽略的警告
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
        });
    }
}
