// <copyright file="DefaultFileFactory.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Store.Services;

[InjectOnScoped]
public class DefaultFileFactory : IFileStoreFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DefaultFileFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IFileStore Create(FileVisibility type)
    {
        return _serviceProvider.GetRequiredKeyedService<IFileStore>(type);
    }
}