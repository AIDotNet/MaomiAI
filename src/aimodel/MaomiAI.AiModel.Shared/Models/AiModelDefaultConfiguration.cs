// <copyright file="AiModelDefaultConfiguration.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.AiModel.Shared.Models;

/// <summary>
/// AI 模型默认配置.
/// </summary>
public class AiModelDefaultConfiguration
{
    /// <summary>
    /// 模型 id.
    /// </summary>
    public Guid ModelId { get; init; }

    /// <summary>
    /// 名称.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// AI 服务商.
    /// </summary>
    public string Provider { get; init; }

    /// <summary>
    /// AI 模型的功能，判断是否多模态.
    /// </summary>
    public AiModelFunction[] AiFunction { get; init; }

    /// <summary>
    /// 支持 function call.
    /// </summary>
    public bool IsSupportFunctionCall { get; init; }

    /// <summary>
    /// 支持图片.
    /// </summary>
    public bool IsSupportImg { get; init; }
}