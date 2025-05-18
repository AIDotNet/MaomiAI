// <copyright file="SetEmbeddingGenerationDocumentTaskCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Azure.Core;
using DocumentFormat.OpenXml.Office2016.Excel;
using Maomi.MQ;
using MaomiAI.AiModel.Shared.Helpers;
using MaomiAI.AiModel.Shared.Models;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Document.Shared.Models;
using MaomiAI.Infra;
using MaomiAI.Infra.Helpers;
using MaomiAI.Infra.Service;
using MaomiAI.Store.Clients;
using MaomiAI.Store.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.Configuration;

namespace MaomiAI.Document.Core.Services;

[InjectOnScoped]
public class CustomKernelMemoryBuilder
{
    private readonly SystemOptions _systemOptions;

    public void ConfigEmbeddingModel(IKernelMemoryBuilder kernelMemoryBuilder, AiEndpoint endpoint, WikiConfig wikiConfig)
    {
        if (!endpoint.AiFunction.Contains(AiModelFunction.TextEmbeddingGeneration))
        {
            throw new BusinessException("{0} 不支持 TextEmbeddingGeneration.", endpoint.Name);
        }

        if (endpoint.Provider == AiProvider.OpenAI)
        {
            kernelMemoryBuilder.WithOpenAITextEmbeddingGeneration(new OpenAIConfig
            {
                EmbeddingModel = endpoint.DeploymentName,
                Endpoint = endpoint.Endpoint,
                APIKey = endpoint.Key,

                MaxEmbeddingBatchSize = maxEmbeddingBatchSize,
                MaxRetries = maxRetries,
                EmbeddingModelMaxTokenTotal = endpoint.EmbeddinMaxToken,
                TextModelMaxTokenTotal = endpoint.TextMaxToken,
                EmbeddingDimensions = embeddingDimensions,
                EmbeddingModelTokenizer = tokenizer
            });
        }
        else if (endpoint.Provider == AiProvider.AzureOpenAI)
        {
            kernelMemoryBuilder.WithAzureOpenAITextEmbeddingGeneration(new AzureOpenAIConfig
            {
                Deployment = endpoint.DeploymentName,
                Endpoint = endpoint.Endpoint,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey,
                APIKey = endpoint.Key,
                APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,

                MaxEmbeddingBatchSize = maxEmbeddingBatchSize,
                MaxRetries = maxRetries,
                MaxTokenTotal = endpoint.EmbeddinMaxToken,
                EmbeddingDimensions = embeddingDimensions,
                Tokenizer = tokenizer
            });
        }
        else
        {
            throw new BusinessException("暂不支持此模型供应商");
        }
    }
}