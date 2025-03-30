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
/// 配置 MVC .
/// </summary>
public class ConfigureMVCModule : IModule
{
    private readonly IConfiguration _configuration;
    private readonly SystemOptions _systemOptions;

    public ConfigureMVCModule(IConfiguration configuration)
    {
        _configuration = configuration;
        _systemOptions = configuration.Get<SystemOptions>()!;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        context.Services.AddControllers(options =>
        {
            options.Filters.Add<MVCExceptionFilter>();
        })
            .ConfigureApplicationPartManager(apm =>
            {
                apm.ApplicationParts.Add(new AssemblyPart(typeof(UserApiModule).Assembly));
            });

    }
}
