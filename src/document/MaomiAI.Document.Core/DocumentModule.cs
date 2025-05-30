﻿// <copyright file="DocumentModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Document.Api;

namespace MaomiAI.Infra;

/// <summary>
/// InfraCoreModule.
/// </summary>
[InjectModule<DocumentApiModule>]
public class DocumentModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
    }
}