// <copyright file="StoreCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra;
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

    private readonly SystemOptions _systemOptions;

    public StoreCoreModule(SystemOptions systemOptions)
    {
        _systemOptions = systemOptions;
    }

    /// <inheritdoc />
    public void ConfigureServices(ServiceContext context)
    {
        var publicEndpoint = new Uri(_systemOptions.PublicStore.Endpoint);
        var privateEndpoint = new Uri(_systemOptions.PrivateStore.Endpoint);

        if (!_systemOptions.PublicStore.ForcePathStyle)
        {
            publicEndpoint = new Uri($"{publicEndpoint.Scheme}://{_systemOptions.PublicStore.Bucket}.{publicEndpoint.Host}");
        }
        else
        {
            publicEndpoint = new Uri($"{publicEndpoint.Scheme}://{publicEndpoint.Host}/{_systemOptions.PublicStore.Bucket}");
        }

        if (!_systemOptions.PrivateStore.ForcePathStyle)
        {
            privateEndpoint = new Uri($"{privateEndpoint.Scheme}://{_systemOptions.PrivateStore.Bucket}.{privateEndpoint.Host}");
        }
        else
        {
            privateEndpoint = new Uri($"{privateEndpoint.Scheme}://{privateEndpoint.Host}/{_systemOptions.PrivateStore.Bucket}");
        }

        context.Services
            .AddRefitClient<IPrivateFileDownClient>(RefitSettings)
            .SetHandlerLifetime(TimeSpan.FromSeconds(100))
            .ConfigureHttpClient(c => c.BaseAddress = privateEndpoint);

        context.Services.AddRefitClient<IPublicFileDownClient>(RefitSettings)
            .SetHandlerLifetime(TimeSpan.FromSeconds(100))
            .ConfigureHttpClient(c => c.BaseAddress = publicEndpoint);
    }
}