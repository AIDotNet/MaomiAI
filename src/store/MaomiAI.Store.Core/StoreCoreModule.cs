// <copyright file="StoreCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Store;

/// <summary>
/// 存储核心模块.
/// </summary>
//[InjectModule<StoreLocalFileSystemModule>]
[InjectModule<StoreS3Module>]
[InjectModule<StoreApiModule>]
public class StoreCoreModule : IModule
{
    /// <inheritdoc />
    public void ConfigureServices(ServiceContext context)
    {
    }
}