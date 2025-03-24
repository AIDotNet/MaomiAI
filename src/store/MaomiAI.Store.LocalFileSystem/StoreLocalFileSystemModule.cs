// <copyright file="StoreCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;

namespace MaomiAI.Store
{
    public class StoreLocalFileSystemModule : IModule
    {
        public void ConfigureServices(ServiceContext context)
        {
            //var systemOptions = context.Configuration.Get<SystemOptions>();

            //ArgumentNullException.ThrowIfNull(systemOptions, nameof(systemOptions));

            //if (systemOptions.PublicStore.Type == "Local")
            //{
            //    var localStore = new LocalStore(
            //        context.Services.GetRequiredService<IAESProvider>(),
            //        systemOptions.Server,
            //        systemOptions.PublicStore.Path
            //    );
            //    context.Services.AddKeyedScoped<IFileStore>(FileStoreType.Public, (s, _) => localStore);

            //    // Register SecurePhysicalFileProvider
            //    var storePath = Path.Combine(Directory.GetCurrentDirectory(), "files");
            //    context.Services.AddSingleton(sp => new LocalPhysicalFileProvider(
            //        storePath,
            //        localStore,
            //        sp.GetRequiredService<IHttpContextAccessor>()
            //    ));
            //}

            //if (systemOptions.PrivateStore.Type == "Local")
            //{
            //    context.Services.AddKeyedScoped<IFileStore>(FileStoreType.Private, (s, _) =>
            //    {
            //        return new LocalStore(
            //            s.GetRequiredService<IAESProvider>(),
            //            systemOptions.Server,
            //            systemOptions.PrivateStore.Path
            //        );
            //    });
            //}
        }
    }
}