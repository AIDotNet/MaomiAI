// <copyright file="AiProviderHelper.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;

namespace MaomiAI.AiModel.Shared.Helpers;

public static class AiProviderHelper
{
    /// <summary>
    /// 获取服务商名称.
    /// </summary>
    /// <param name="provider">服务商.</param>
    /// <returns>服务商名称.</returns>
    public static string GetProviderName(this AiProvider provider)
    {
        return provider switch
        {
            AiProvider.Custom => "自定义",
            AiProvider.OpenAI => "OpenAI",
            AiProvider.AzureOpenAI => "Azure OpenAI",
            AiProvider.Deepseek => "Deepseek",
            AiProvider.Anthropic => "Anthropic",
            AiProvider.Google => "Google",
            AiProvider.Cohere => "Cohere",
            AiProvider.Mistral => "Mistral",
            AiProvider.HuggingFace => "HuggingFace",
            _ => throw new ArgumentOutOfRangeException(nameof(provider), provider, null)
        };
    }

    /// <summary>
    /// 通过服务商名称获取对应的枚举值.
    /// </summary>
    /// <param name="name">服务商名称.</param>
    /// <returns>对应的 AiProvider 枚举值.</returns>
    /// <exception cref="ArgumentException">如果名称无效则抛出异常.</exception>
    public static AiProvider GetProviderByName(string name)
    {
        foreach (var info in Providers)
        {
            if (string.Equals(info.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                return info.Provider;
            }
        }

        throw new ArgumentOutOfRangeException(nameof(name), name, null);
    }

    /// <summary>
    /// 获取支持的供应商列表.
    /// </summary>
    public static readonly IReadOnlyCollection<AiProviderInfo> Providers = new AiProviderInfo[]
    {
        new AiProviderInfo
        {
            Provider = AiProvider.Custom,
            Name = "自定义",
            Description = "自定义服务商",
            Icon = "custom.png",
            DefaultEndpoint = string.Empty
        },
        new AiProviderInfo
        {
            Provider = AiProvider.OpenAI,
            Name = "OpenAI",
            Description = "OpenAI 服务商",
            Icon = "openai.png",
            DefaultEndpoint = "https://api.openai.com/v1"
        },
        new AiProviderInfo
        {
            Provider = AiProvider.AzureOpenAI,
            Name = "Azure OpenAI",
            Description = "Azure OpenAI 服务商",
            Icon = "azure_openai.png",
            DefaultEndpoint = "https://{endpoint}.openai.azure.com"
        },
        new AiProviderInfo
        {
            Provider = AiProvider.Deepseek,
            Name = "Deepseek",
            Description = "Deepseek 服务商",
            Icon = "deepseek.png",
            DefaultEndpoint = "https://api.deepseek.com/v1"
        },
        new AiProviderInfo
        {
            Provider = AiProvider.Anthropic,
            Name = "Anthropic",
            Description = "Anthropic 服务商",
            Icon = "anthropic.png",
            DefaultEndpoint = "https://api.anthropic.com/v1"
        },
        new AiProviderInfo
        {
            Provider = AiProvider.Google,
            Name = "Google",
            Description = "Google 服务商",
            Icon = "google.png",
            DefaultEndpoint = "https://{api}.googleapis.com/v1/"
        },
        new AiProviderInfo
        {
            Provider = AiProvider.Cohere,
            Name = "Cohere",
            Description = "Cohere 服务商",
            Icon = "cohere.png",
            DefaultEndpoint = "https://api.cohere.ai/v1"
        },
        new AiProviderInfo
        {
            Provider = AiProvider.Mistral,
            Name = "Mistral",
            Description = "Mistral 服务商",
            Icon = "mistral.png",
            DefaultEndpoint = "https://api.mistral.ai/v1"
        },
        new AiProviderInfo
        {
            Provider = AiProvider.HuggingFace,
            Name = "HuggingFace",
            Description = "HuggingFace 服务商",
            Icon = "huggingface.png",
            DefaultEndpoint = "https://api-inference.huggingface.co"
        }
    };
}

public static class AiModelFunctionHelper
{
    /// <summary>
    /// 判断是否具备多模态功能.
    /// </summary>
    /// <param name="function">功能.</param>
    /// <returns>是否具备多模态功能.</returns>
    public static bool IsMultiModal(this AiModelFunction function)
    {
        return (function & (AiModelFunction.TextToImage | AiModelFunction.TextToAudio)) != 0;
    }

}