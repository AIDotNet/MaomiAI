// <copyright file="DatabaseCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Microsoft.Extensions.Configuration;

namespace MaomiAI.Public;

/// <summary>
/// PublicApiModule.
/// </summary>
public class PublicApiModule : IModule
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublicApiModule"/> class.
    /// </summary>
    /// <param name="configuration">配置.</param>
    public PublicApiModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
    }
}