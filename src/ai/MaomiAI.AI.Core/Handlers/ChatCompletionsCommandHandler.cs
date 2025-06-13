// <copyright file="Class1.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AI.Commands;
using MaomiAI.AI.Core.ChatCompletion;
using MaomiAI.Infra.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Org.BouncyCastle.Utilities.Collections;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Diagnostics;

namespace MaomiAI.AI.Core.Handlers;

// 后续推送统计

public class ChatCompletionsCommandHandler : IStreamRequestHandler<ChatCompletionsCommand, string>
{
    private readonly IRedisDatabase _redisDatabase;
    private readonly IServiceProvider _serviceProvider;

    public ChatCompletionsCommandHandler(IRedisDatabase redisDatabase, IServiceProvider serviceProvider)
    {
        _redisDatabase = redisDatabase;
        _serviceProvider = serviceProvider;
    }

    public async IAsyncEnumerable<string> Handle(ChatCompletionsCommand request, CancellationToken cancellationToken)
    {
        var kernelBuilder = Kernel.CreateBuilder();
        var chatCompletionConfigurator = _serviceProvider.GetKeyedService<IChatCompletionConfigurator>(request.Endpoint.Provider);
        if (chatCompletionConfigurator == null)
        {
            throw new BusinessException("暂不支持该模型");
        }

        var kernel = chatCompletionConfigurator.AddChatCompletion(kernelBuilder, request.Endpoint)
            .Build();

        // TODO: 添加知识库
        // TODO: 添加插件
        // TODO: 影响因素

        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        // 后续加上 try ，断开连接后继续处理后续代码

        // 流式
        var responseStream = chatCompletionService.GetStreamingChatMessageContentsAsync(
            chatHistory: request.ChatHistory,
            kernel: kernel,
            executionSettings: request.ExecutionSettings,
            cancellationToken: cancellationToken);

        var responseContent = new System.Text.StringBuilder();
        Stopwatch stopwatch = Stopwatch.StartNew();

        await foreach (var chunk in responseStream)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                yield break;
            }

            if (!string.IsNullOrEmpty(chunk.Content))
            {
                responseContent.Append(chunk.Content);
                yield return chunk.Content;
            }

            var streamingChatCompletion = chunk.InnerContent as OpenAI.Chat.StreamingChatCompletionUpdate;
            if (streamingChatCompletion != null)
            {
                var usage = streamingChatCompletion.Usage;
                if (streamingChatCompletion.FinishReason != null && streamingChatCompletion.FinishReason == OpenAI.Chat.ChatFinishReason.Stop)
                {
                    // 统计耗时.
                    stopwatch.Stop();
                }

                if (usage != null)
                {
                    // 统计 tokens 数量
                }
            }
        }

        if (cancellationToken.IsCancellationRequested)
        {
            // todo: 如果结束
            yield return string.Empty;
        }

        request.ChatHistory.AddAssistantMessage(responseContent.ToString());
        await _redisDatabase.Database.HashSetAsync(
            key: $"chat:{request.ChatId}",
            hashFields: request.ChatHistory.Select(x => new HashEntry(x.ModelId, x.ToRedisValue())).ToArray());
    }
}
