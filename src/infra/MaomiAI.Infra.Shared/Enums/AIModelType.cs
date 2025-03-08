// <copyright file="AIModelType.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra.Enums;

/// <summary>
/// AI 模型类型.
/// </summary>
public enum AIModelType
{
    /// <summary>
    /// 对话模型.
    /// </summary>
    ChatCompletion = 0,

    /// <summary>
    /// 文本生成模型.
    /// </summary>
    TextGeneration = 1,

    /// <summary>
    /// 嵌入模型.
    /// </summary>
    TextEmbeddingGeneration = 2,

    /// <summary>
    /// 文本生成图像.
    /// </summary>
    TextToImage = 3,

    /// <summary>
    /// 文本生成音频.
    /// </summary>
    TextToAudio = 4,

    /// <summary>
    /// 语音识别.
    /// </summary>
    AudioToText = 5
}
