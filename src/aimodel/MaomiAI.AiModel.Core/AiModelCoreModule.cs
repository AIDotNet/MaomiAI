// <copyright file="AiModelCoreModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Api;

namespace MaomiAI.AiModel.Core;

/// <summary>
/// AiModelCoreModule.
/// </summary>
[InjectModule<AiModelApiModule>]
public class AiModelCoreModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
    }
}
