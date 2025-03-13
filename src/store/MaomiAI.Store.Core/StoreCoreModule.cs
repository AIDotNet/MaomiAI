// <copyright file="StoreCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;

namespace MaomiAI.Store;

[InjectModule<StoreLocalFileSystemModule>]
[InjectModule<StoreS3Module>]
public class StoreCoreModule : IModule
{
    public void ConfigureServices(ServiceContext context)
    {
    }
}
