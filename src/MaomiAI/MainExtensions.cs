using Maomi;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Extensions.Logging;

namespace MaomiAI;

public static class MainExtensions
{
    public static IHostApplicationBuilder UseMaomiAI(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IConfigurationManager>(builder.Configuration);

        builder.Services.AddModule<MainModule>();

        builder.Services.AddSerilog((services, configuration) =>
        {
            configuration.ReadFrom.Services(services);
#if DEBUG
            configuration.ReadFrom.Configuration(builder.Configuration);
#else
            // 使用 json 收集时，忽略日志模板
            ctx.Configuration["Serilog:WriteTo:0:Args:outputTemplate"] = "";

            // 以 json 格式化输出
            configuration.WriteTo.Console(new RenderedCompactJsonFormatter())
            .ReadFrom.Configuration(ctx.Configuration);
#endif
        });

        builder.Services.AddModule<MainModule>();

        return builder;
    }
}
