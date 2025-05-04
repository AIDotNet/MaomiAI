// <copyright file="InfraConfigurationModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Service;
using MaomiAI.Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

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

        ServiceProvider? ioc = context.Services.BuildServiceProvider();

        IConfigurationManager? configurationBuilder = ioc.GetRequiredService<IConfigurationManager>();

        ImportSystemConfiguration(context, configurationBuilder);
        ImportLoggerConfiguration(context, configurationBuilder);
        ConfigureRsaPrivate(context, configurationBuilder);

        SystemOptions? systemOptions = configurationBuilder.Get<SystemOptions>() ??
                                       throw new FormatException("The system configuration cannot be loaded.");
        context.Services.AddSingleton(systemOptions);

        context.Services.AddSingleton<IAESProvider>(s => { return new AESProvider(systemOptions.AES); });
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
        // todo: 将 MAI_CONFIG 改成指定目录，而不是指定配置文件.
        // 指定环境变量从文件导入配置
        string? configurationFilePath = Environment.GetEnvironmentVariable("MAI_CONFIG");
        if (string.IsNullOrWhiteSpace(configurationFilePath))
        {
            return;
        }

        string? fileType = Path.GetExtension(configurationFilePath);
        if (".json".Equals(fileType, StringComparison.OrdinalIgnoreCase))
        {
            configurationBuilder.AddJsonFile(configurationFilePath);
        }
        else if (".yaml".Equals(fileType, StringComparison.OrdinalIgnoreCase))
        {
            configurationBuilder.AddYamlFile(configurationFilePath);
        }
        else if (".conf".Equals(fileType, StringComparison.OrdinalIgnoreCase))
        {
            configurationBuilder.AddIniFile(configurationFilePath);
        }
        else
        {
            _logger.LogWarning("The current file type cannot be imported,`MAI_CONFIG={File}`.",
                configurationFilePath);
            throw new ArgumentException(
                $"The current file type cannot be imported,`MAI_CONFIG={configurationFilePath}`.");
        }
    }

    // 导入日志配置文件.
    private void ImportLoggerConfiguration(ServiceContext context, IConfigurationBuilder configurationBuilder)
    {
        if (!File.Exists(AppConsts.LoggerJson))
        {
            if (Directory.Exists(AppConsts.DefaultConfigsPath))
            {
                File.Copy(AppConsts.DefaultLoggerJson, AppConsts.LoggerJson);
            }
        }

        if (File.Exists(AppConsts.LoggerJson))
        {
            configurationBuilder.AddJsonFile(AppConsts.LoggerJson);
        }
    }

    private void ConfigureRsaPrivate(ServiceContext context, IConfigurationBuilder configurationBuilder)
    {
        if (!File.Exists(AppConsts.RSA))
        {
            using RSA? rsa = RSA.Create(2048);
            string rsaPrivate = rsa.ExportPkcs8PrivateKeyPem();
            File.WriteAllText(AppConsts.RSA, rsaPrivate);
            context.Services.AddSingleton<IRsaProvider>(s => { return new RsaProvider(rsaPrivate); });
        }
        else
        {
            string? rsaPrivate = File.ReadAllText(Path.Combine(AppConsts.AppPath, AppConsts.RSA));
            context.Services.AddSingleton<IRsaProvider>(s => { return new RsaProvider(rsaPrivate); });
        }
    }
}