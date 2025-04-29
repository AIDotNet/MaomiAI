using MaomiAI.Infra;
using MaomiAI.User.Core.Services;
using Microsoft.Extensions.Options;
using Serilog;

namespace MaomiAI;

public static class MainExtensions
{
    public static IHostApplicationBuilder UseMaomiAI(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IConfigurationManager>(builder.Configuration);

        // 注册系统配置
        builder.Services.Configure<SystemOptions>(builder.Configuration);
        builder.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<SystemOptions>>().Value);

        builder.Services.AddSerilog((services, configuration) =>
        {
            configuration.ReadFrom.Services(services);
            configuration.ReadFrom.Configuration(builder.Configuration);
        });

        builder.Services.AddModule<MainModule>();

        return builder;
    }

    public static WebApplication UseMaomiAIMiddleware(this WebApplication app)
    {
        // 使用认证中间件
        app.UseMiddleware<CustomAuthorizaMiddleware>();

        return app;
    }
}