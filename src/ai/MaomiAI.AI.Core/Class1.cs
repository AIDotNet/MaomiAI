// <copyright file="Class1.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using Maomi;
using MaomiAI.AiModel.Shared.Models;
using MaomiAI.Infra.Exceptions;
using MediatR;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.ClientModel;

namespace MaomiAI.AI.Core;

public class ChatCompletionsCommandHandler : IStreamRequestHandler<ChatCompletionsCommand, string>
{
    private readonly IRedisDatabase _redisDatabase;
    private readonly KernelBuilderFactory _kernelBuilderFactory;

    public ChatCompletionsCommandHandler(IRedisDatabase redisDatabase, KernelBuilderFactory kernelBuilderFactory)
    {
        _redisDatabase = redisDatabase;
        _kernelBuilderFactory = kernelBuilderFactory;
    }

    public async IAsyncEnumerable<string> Handle(ChatCompletionsCommand request, CancellationToken cancellationToken)
    {
        var kernel = _kernelBuilderFactory.ConfigKernelBuilder(Kernel.CreateBuilder())
            .AddLogger()
            .AddChatCompletion(request.Endpoint)
            .Build();

        // TODO: 添加知识库
        // TODO: 添加插件

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        // TODO: 影响因素

        // 流式
        var responseStream = chatCompletionService.GetStreamingChatMessageContentsAsync(
            chatHistory: request.ChatHistory,
            kernel: kernel);

        var responseContent = new System.Text.StringBuilder();
        await foreach (var chunk in responseStream)
        {
            if (chunk == null || chunk.Content == null)
            {
                continue;
            }

            responseContent.Append(chunk);

            yield return chunk.Content;
        }

        request.ChatHistory.AddAssistantMessage(responseContent.ToString());
        await _redisDatabase.Database.StringSetAsync(key: $"chat:{request.ChatId}", value: request.ChatHistory.ToRedisValue());

    }
}

public class ChatCompletionsCommand : IStreamRequest<string>
{
    public string ChatId { get; init; }
    public AiEndpoint Endpoint { get; init; }
    public ChatHistory ChatHistory { get; init; } = new ChatHistory();
}

[InjectOnScoped]
public class KernelBuilderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public KernelBuilderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public KernelBuilderService ConfigKernelBuilder(IKernelBuilder kernelBuilder)
    {
        return new KernelBuilderService(kernelBuilder, _serviceProvider);
    }
}

public class KernelBuilderService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IKernelBuilder _kernelBuilder;

    public KernelBuilderService(IKernelBuilder kernelBuilder, IServiceProvider serviceProvider)
    {
        _kernelBuilder = kernelBuilder;
        _serviceProvider = serviceProvider;
    }

    public KernelBuilderService AddLogger()
    {

        //_kernelBuilder.Services.AddLogging (o =>
        //{
        //    o.AddConfiguration(_kernelBuilder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetSection("Logging"));
        //});
        return this;
    }

    public KernelBuilderService AddChatCompletion(AiEndpoint endpoint)
    {
        if (endpoint.Provider == AiProvider.OpenAI)
        {
            _kernelBuilder.AddOpenAIChatCompletion(
                apiKey: endpoint.Key,
                endpoint: new Uri(endpoint.Endpoint),
                modelId: endpoint.Name,
                serviceId: "MaomiAI");
        }
        else if (endpoint.Provider == AiProvider.Azure || endpoint.Provider == AiProvider.AzureAI)
        {
            _kernelBuilder.AddAzureOpenAIChatCompletion(
                deploymentName: endpoint.DeploymentName,
                apiKey: endpoint.Key,
                endpoint: endpoint.Endpoint,
                modelId: endpoint.Name,
                serviceId: "MaomiAI");
        }
        else if (endpoint.Provider == AiProvider.Custom || endpoint.Provider == AiProvider.DeepSeek)
        {
            var openAIClientCredential = new ApiKeyCredential(endpoint.Key);
            var openAIClientOption = new OpenAIClientOptions
            {
                Endpoint = new Uri(endpoint.Endpoint),
            };

            var openapiClient = new OpenAIClient(openAIClientCredential, openAIClientOption);
            _kernelBuilder
                .AddOpenAIChatCompletion(endpoint.Name, openapiClient, serviceId: "MaomiAI");
        }
        else
        {
            throw new BusinessException("不支持该模型供应商") { StatusCode = 409 };
        }

        return this;
    }

    public Kernel Build()
    {
        return _kernelBuilder.Build();
    }
}