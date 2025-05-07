// <copyright file="ConfigureLoggerModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra;
using Microsoft.AspNetCore.HttpLogging;

namespace MaomiAI.Modules;

/// <summary>
/// 配置日志.
/// </summary>
public class ConfigureLoggerModule : IModule
{
    private readonly IConfiguration _configuration;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureLoggerModule"/> class.
    /// </summary>
    /// <param name="configuration"></param>
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
        // todo: 忽略 swagger
        context.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestQuery | HttpLoggingFields.RequestProtocol
#if DEBUG
            | HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody
#endif
            | HttpLoggingFields.ResponseStatusCode
            ;

            logging.CombineLogs = true;
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }
}
