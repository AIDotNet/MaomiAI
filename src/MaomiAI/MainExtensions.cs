using Maomi;
using Serilog;

namespace MaomiAI;

public static class MainExtensions
{
    public static IHostApplicationBuilder UseMaomiAI(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IConfigurationManager>(builder.Configuration);

        builder.Services.AddSerilog((services, configuration) =>
        {
            configuration.ReadFrom.Services(services);
            configuration.ReadFrom.Configuration(builder.Configuration);
        });

        builder.Services.AddModule<MainModule>();

        return builder;
    }
}
