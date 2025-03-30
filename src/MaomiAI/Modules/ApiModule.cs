using Maomi;
using MaomiAI.Filters;
using MaomiAI.Infra;
using MaomiAI.User.Api;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi;
using static MaomiAI.MainModule;

namespace MaomiAI.Modules;

/// <summary>
/// 聚合 API 项目中的各个子模块.
/// </summary>
[InjectModule<ConfigureLoggerModule>]
[InjectModule<ConfigureMVCModule>]
[InjectModule<ConfigureOpenApiModule>]
[InjectModule<ConfigureMediatRModule>]
public class ApiModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
    }
}
