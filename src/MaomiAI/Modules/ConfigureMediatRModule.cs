using Maomi;
using MaomiAI.Infra;

namespace MaomiAI.Modules;

/// <summary>
/// 配置 MediatR .
/// </summary>
public class ConfigureMediatRModule : IModule
{
    private readonly IConfiguration _configuration;
    private readonly SystemOptions _systemOptions;

    public ConfigureMediatRModule(IConfiguration configuration)
    {
        _configuration = configuration;
        _systemOptions = configuration.Get<SystemOptions>()!;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {

        context.Services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(context.Modules.Select(x => x.Assembly).ToArray());
        });
    }
}