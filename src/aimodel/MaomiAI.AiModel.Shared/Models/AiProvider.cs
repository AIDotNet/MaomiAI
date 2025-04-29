// <copyright file="AiProvider.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.AiModel.Shared.Models;

/// <summary>
/// AI 服务提供商，服务商来源.
/// </summary>
public enum AiProvider
{
    /// <summary>
    /// 自定义.
    /// </summary>
    Custom = 0,

    /// <summary>
    /// OpenAI.
    /// </summary>
    OpenAI = 1,

    /// <summary>
    /// AzureOpenAI.
    /// </summary>
    AzureOpenAI = 2,

    /// <summary>
    /// Deepseek.
    /// </summary>
    Deepseek = 3,
}
