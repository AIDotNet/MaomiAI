using Maomi;

namespace MaomiAI.Modules;

/// <summary>
/// 聚合 API 项目中的各个子模块.
/// </summary>
[InjectModule<ConfigureLoggerModule>]
[InjectModule<ConfigureMVCModule>]
[InjectModule<ConfigureOpenApiModule>]
[InjectModule<ConfigureMediatRModule>]
[InjectModule<FastEndpointModule>]
public class ApiModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
    }
}
