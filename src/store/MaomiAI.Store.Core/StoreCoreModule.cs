// <copyright file="StoreCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Clients;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Text.Json;

namespace MaomiAI.Store;

/// <summary>
/// 存储核心模块.
/// </summary>
//[InjectModule<StoreLocalFileSystemModule>]
[InjectModule<StoreS3Module>]
[InjectModule<StoreApiModule>]
public class StoreCoreModule : IModule
{
    private static readonly RefitSettings RefitSettings = new RefitSettings()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip
        }),
        Buffered = true
    };

    /// <inheritdoc />
    public void ConfigureServices(ServiceContext context)
    {
        context.Services.AddRefitClient<IFileDownClient>(RefitSettings)
            .SetHandlerLifetime(TimeSpan.FromSeconds(2));
    }
}