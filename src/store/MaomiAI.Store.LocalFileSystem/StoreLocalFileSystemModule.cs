// <copyright file="StoreCoreModule.cs" company="MaomiAI">
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

public class StoreLocalFileSystemModule : IModule
{
    public void ConfigureServices(ServiceContext context)
    {
        var systemOptions = context.Configuration.Get<SystemOptions>();

        ArgumentNullException.ThrowIfNull(systemOptions, nameof(systemOptions));

        if (systemOptions.PublicStore.Type == "Local")
        {
            context.Services.AddKeyedScoped<IFileStore>(FileStoreType.Public, (s, _) =>
            {
                return new LocalStore(systemOptions.Server, systemOptions.PublicStore.Path);
            });
        }

        if (systemOptions.PrivateStore.Type == "Local")
        {
            context.Services.AddKeyedScoped<IFileStore>(FileStoreType.Private, (s, _) =>
            {
                return new LocalStore(systemOptions.Server, systemOptions.PrivateStore.Path);
            });
        }
    }
}
