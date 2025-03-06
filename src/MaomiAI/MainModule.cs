// <copyright file="MainModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using MaomiAI.Database;
using MaomiAI.Infra;
using Microsoft.AspNetCore.HttpLogging;

namespace MaomiAI;

/// <summary>
/// MainModule.
/// </summary>
[InjectModule<InfraCoreModule>]
[InjectModule<DatabaseCoreModule>]
[InjectModule<EmbeddingCoreModule>]
[InjectModule<DocumentModule>]
public class MainModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        ConfigureHttpLogging(context);
    }

    // 配置 http 请求日志.
    private static void ConfigureHttpLogging(ServiceContext context)
    {
        // todo: 后续是否允许在配置文件指定 LoggingFields 参数
        context.Services.AddHttpLogging(logging =>
        {
            //logging.LoggingFields = HttpLoggingFields.All;
        });
    }
}
