// <copyright file="AiNotKeyEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.AiModel.Shared.Models;

/// <summary>
/// AI 模型.
/// </summary>
public class AiNotKeyEndpoint
{
    /// <summary>
    /// id.
    /// </summary>
    public Guid Id { get; init; } = Guid.Empty;

    /// <summary>
    /// 名称.
    /// </summary>
    public string Name { get; init; } = default!;

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

    /// <summary>
    /// 请求端点.
    /// </summary>
    public string endpoint { get; init; } = default!;

    /// <summary>
    /// 模型部署 id 或 name.
    /// </summary>
    public string ModelId { get; init; } = default!;

    /// <summary>
    /// 模型部署名称，可跟 ModelId 一样，兼容 Azure Open AI.
    /// </summary>
    public string DeploymentName { get; init; } = default!;

    /// <summary>
    /// 文本模型最大支持上下文token.
    /// </summary>
    public int TextMaxToken { get; set; }

    /// <summary>
    /// 嵌入模型最大支持token数量.
    /// </summary>
    public int EmbeddinMaxToken { get; set; }
}