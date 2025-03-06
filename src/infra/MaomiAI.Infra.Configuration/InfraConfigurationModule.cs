// <copyright file="InfraConfigurationModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Infra;

/*
 配置文件都应该放在 configs/logger.json 下，这样可以方便的进行配置文件的管理.
 考虑到适配 docker 等环境，如果 configs 不存在默认需要的文件，则需要自动生成默认.
 */

/// <summary>
/// InfraConfigurationModule.
/// </summary>
public class InfraConfigurationModule : IModule
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="InfraConfigurationModule"/> class.
    /// </summary>
    /// <param name="loggerFactory"></param>
    public InfraConfigurationModule(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(Constant.BaseLoggerName);
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        CheckConfigsDirectory(context);

        var ioc = context.Services.BuildServiceProvider();
        var configurationBuilder = ioc.GetRequiredService<IConfigurationManager>();

        ImportSystemConfiguration(context, configurationBuilder);
        ImportLoggerConfiguration(context, configurationBuilder);
    }

    // 检查配置目录是否存在.
    private void CheckConfigsDirectory(ServiceContext context)
    {
        if (!Directory.Exists("configs"))
        {
            Directory.CreateDirectory("configs");
        }
    }

    // 导入系统配置.
    private void ImportSystemConfiguration(ServiceContext context, IConfigurationBuilder configurationBuilder)
    {
        // 指定环境变量从文件导入配置
        var configurationFilePath = Environment.GetEnvironmentVariable("MAI_CONFIG");
        if (string.IsNullOrWhiteSpace(configurationFilePath))
        {
            return;
        }

        var fileType = Path.GetExtension(configurationFilePath);
        if ("json".Equals(fileType, StringComparison.OrdinalIgnoreCase))
        {
            configurationBuilder.AddJsonFile(configurationFilePath);
        }
        else if ("yaml".Equals(fileType, StringComparison.OrdinalIgnoreCase))
        {
            configurationBuilder.AddYamlFile(configurationFilePath);
        }
        else if (".conf".Equals(fileType, StringComparison.OrdinalIgnoreCase))
        {
            configurationBuilder.AddIniFile(configurationFilePath);
        }

        _logger.LogWarning("The current file type cannot be imported,`MAI_CONFIG={File}`.", configurationFilePath);
    }

    // 导入日志配置文件.
    private void ImportLoggerConfiguration(ServiceContext context, IConfigurationBuilder configurationBuilder)
    {
        if (!File.Exists("configs/logger.json"))
        {
            File.Copy("default_configs/logger.json", "configs/logger.json");
        }

        configurationBuilder.AddJsonFile("configs/logger.json");
    }
}