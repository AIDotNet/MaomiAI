// <copyright file="StoreS3Module.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using MaomiAI.Infra;
using MaomiAI.Store.Enums;
using MaomiAI.Store.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Store;

/// <summary>
/// S3 存储模块.
/// </summary>
public class StoreS3Module : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        SystemOptions? systemOptions = context.Configuration.Get<SystemOptions>();

        ArgumentNullException.ThrowIfNull(systemOptions, nameof(systemOptions));

        context.Services.AddKeyedScoped<IFileStore>(
            FileStoreType.Public,
            (s, _) => { return new S3Store(systemOptions.PublicStore); });

        context.Services.AddKeyedScoped<IFileStore>(
            FileStoreType.Private,
            (s, _) => { return new S3Store(systemOptions.PrivateStore); });
    }
}