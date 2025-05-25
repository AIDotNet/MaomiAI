// <copyright file="PromptType.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MaomiAI.Prompt.Models;

/// <summary>
/// 提示词类型.
/// </summary>
public enum PromptType
{
    /// <summary>
    /// 学术。
    /// </summary>
    [JsonPropertyName("academic")]
    [EnumMember(Value = "academic")]
    Academic,

    /// <summary>
    /// 职业。
    /// </summary>
    [JsonPropertyName("career")]
    [EnumMember(Value = "career")]
    Career,

    /// <summary>
    /// 文案。
    /// </summary>
    [JsonPropertyName("copywriting")]
    [EnumMember(Value = "copywriting")]
    Copywriting,

    /// <summary>
    /// 设计。
    /// </summary>
    [JsonPropertyName("design")]
    [EnumMember(Value = "design")]
    Design,

    /// <summary>
    /// 教育。
    /// </summary>
    [JsonPropertyName("education")]
    [EnumMember(Value = "education")]
    Education,

    /// <summary>
    /// 情感。
    /// </summary>
    [JsonPropertyName("emotion")]
    [EnumMember(Value = "emotion")]
    Emotion,

    /// <summary>
    /// 娱乐。
    /// </summary>
    [JsonPropertyName("entertainment")]
    [EnumMember(Value = "entertainment")]
    Entertainment,

    /// <summary>
    /// 游戏。
    /// </summary>
    [JsonPropertyName("gaming")]
    [EnumMember(Value = "gaming")]
    Gaming,

    /// <summary>
    /// 通用。
    /// </summary>
    [JsonPropertyName("generic")]
    [EnumMember(Value = "generic")]
    Generic,

    /// <summary>
    /// 生活。
    /// </summary>
    [JsonPropertyName("lifestyle")]
    [EnumMember(Value = "lifestyle")]
    Lifestyle,

    /// <summary>
    /// 商业。
    /// </summary>
    [JsonPropertyName("business")]
    [EnumMember(Value = "business")]
    Business,

    /// <summary>
    /// 办公。
    /// </summary>
    [JsonPropertyName("office")]
    [EnumMember(Value = "office")]
    Office,

    /// <summary>
    /// 编程。
    /// </summary>
    [JsonPropertyName("programming")]
    [EnumMember(Value = "programming")]
    Programming,

    /// <summary>
    /// 翻译。
    /// </summary>
    [JsonPropertyName("translation")]
    [EnumMember(Value = "translation")]
    Translation
}