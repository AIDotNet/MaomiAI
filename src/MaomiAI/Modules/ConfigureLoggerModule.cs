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
/// 配置日志.
/// </summary>
public class ConfigureLoggerModule : IModule
{
    private readonly IConfiguration _configuration;
    private readonly SystemOptions _systemOptions;

    public ConfigureLoggerModule(IConfiguration configuration)
    {
        _configuration = configuration;
        _systemOptions = configuration.Get<SystemOptions>()!;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        ConfigureHttpLogging(context);
    }

    // 配置 http 请求日志.
    private void ConfigureHttpLogging(ServiceContext context)
    {
        // todo: 后续是否允许在配置文件指定 LoggingFields 参数
        context.Services.AddHttpLogging(logging =>
        {
            //logging.LoggingFields = HttpLoggingFields.All;
            logging.CombineLogs = true;
        });
    }

}
