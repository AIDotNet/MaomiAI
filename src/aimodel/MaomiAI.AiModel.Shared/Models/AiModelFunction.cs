// <copyright file="AiModelFunction.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.AiModel.Shared.Models;

/// <summary>
/// AI 模型的功能，判断是否多模态.
/// </summary>
[Flags]
public enum AiModelFunction
{
    /// <summary>
    /// 不具备任何功能，不可使用.
    /// </summary>
    None = 0,

    /// <summary>
    /// 对话模型.
    /// </summary>
    ChatCompletion = 1,

    /// <summary>
    /// 文本生成模型.
    /// </summary>
    TextGeneration = 1 << 1,

    /// <summary>
    /// 嵌入模型.
    /// </summary>
    TextEmbeddingGeneration = 1 << 2,

    /// <summary>
    /// 文本生成图像.
    /// </summary>
    TextToImage = 1 << 3,

    /// <summary>
    /// 文本生成音频.
    /// </summary>
    TextToAudio = 1 << 4,

    /// <summary>
    /// 语音识别.
    /// </summary>
    AudioToText = 1 << 5
}
