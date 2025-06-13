// <copyright file="Class1.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using MaomiAI.AiModel.Shared.Models;
using MaomiAI.Infra.Exceptions;
using MediatR;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.ClientModel;

namespace MaomiAI.AI.Core.ChatCompletion;

[InjectOnScoped(ServiceKey = AiProvider.Azure)]
public class AzureOpenAiChatCompletion : IChatCompletionConfigurator
{
    public IKernelBuilder AddChatCompletion(IKernelBuilder kernelBuilder, AiEndpoint endpoint)
    {
        return kernelBuilder.AddAzureOpenAIChatCompletion(
                deploymentName: endpoint.DeploymentName,
                apiKey: endpoint.Key,
                endpoint: endpoint.Endpoint,
                modelId: endpoint.Name,
                serviceId: "MaomiAI");
    }
}
