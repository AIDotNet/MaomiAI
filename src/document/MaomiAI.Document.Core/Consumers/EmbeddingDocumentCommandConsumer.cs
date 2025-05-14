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
using MaomiAI.Document.Core.Consumers.Events;
using MaomiAI.Document.Core.Services;
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

namespace MaomiAI.Document.Core.Consumers;

[Consumer("embedding_document", Qos = 1)]
public class EmbeddingDocumentCommandConsumer : IConsumer<EmbeddingDocumentEvent>
{
    private readonly DatabaseContext _databaseContext;
    private readonly SystemOptions _systemOptions;
    private readonly CustomKernelMemoryBuilder _customKernelMemoryBuilder;
    private readonly IMediator _mediator;
    private readonly IFileDownClient _fileDownClient;

    public EmbeddingDocumentCommandConsumer(DatabaseContext databaseContext, SystemOptions systemOptions, CustomKernelMemoryBuilder customKernelMemoryBuilder, IMediator mediator, IFileDownClient fileDownClient)
    {
        _databaseContext = databaseContext;
        _systemOptions = systemOptions;
        _customKernelMemoryBuilder = customKernelMemoryBuilder;
        _mediator = mediator;
        _fileDownClient = fileDownClient;
    }

    public async Task ExecuteAsync(MessageHeader messageHeader, EmbeddingDocumentEvent message)
    {
        var documentTask = await _databaseContext.TeamWikiDocumentTasks
             .FirstOrDefaultAsync(x => x.DocumentId == message.DocumentId && x.TaskId == message.TaskId);

        // 不需要处理
        if (documentTask == null || documentTask.State > (int)FileEmbeddingState.Processing)
        {
            return;
        }

        documentTask.State = (int)FileEmbeddingState.Processing;
        documentTask.Message = "任务开始处理";
        _databaseContext.TeamWikiDocumentTasks.Update(documentTask);
        await _databaseContext.SaveChangesAsync();

        var documentFile = await _databaseContext.TeamWikiDocuments
            .Where(x => x.Id == message.DocumentId)
            .Join(_databaseContext.Files, a => a.FileId, b => b.Id, (a, b) => new
            {
                a.FileId,
                b.FileName,
                FilePath = b.ObjectKey,
            }).FirstOrDefaultAsync();

        if (documentFile == null)
        {
            documentTask.State = (int)FileEmbeddingState.Failed;
            documentTask.Message = "文件不存在";
            await _databaseContext.SaveChangesAsync();
            return;
        }

        // 下载文件
        var filePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + Path.GetExtension(documentFile.FileName));
        var fileDownloadInfo = await _mediator.Send(new QueryFileDownloadUrlCommand
        {
            ExpiryDuration = TimeSpan.FromMinutes(5),
            ObjectKeys = new List<string> { documentFile.FilePath }
        });

        using var remoteFileStream = await _fileDownClient.DownloadFileAsync(fileDownloadInfo.Urls.First().Value.ToString());
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await remoteFileStream.CopyToAsync(fileStream);
        }
        await remoteFileStream.DisposeAsync();

        var teamWikiConfig = await _databaseContext.TeamWikiConfigs
        .Where(x => x.TeamId == documentTask.TeamId && x.WikiId == documentTask.WikiId)
        .Join(_databaseContext.TeamAiModels, a => a.EmbeddingModelId, b => b.Id, (a, b) => new
        {
            b.Name,
            b.DeploymentName,
            b.Key,
            b.IsSupportImg,
            b.TextMaxToken,
            b.IsSupportFunctionCall,
            Provider = AiProviderHelper.GetProviderByName(b.AiProvider),
            ModelId = b.ModeId,
            AiFunction = EnumHelper.DecomposeFlags<AiModelFunction>(b.AiModelFunction),
            b.EmbeddinMaxToken,
            b.Endpoint,
            a.EmbeddingDimensions,
            a.EmbeddingModelTokenizer,
            a.EmbeddingBatchSize,
            a.MaxRetries
        }).FirstOrDefaultAsync();

        if (teamWikiConfig == null)
        {
            documentTask.State = (int)FileEmbeddingState.Failed;
            documentTask.Message = "知识库未配置向量化模型";
            await _databaseContext.SaveChangesAsync();
            return;
        }

        var aiEndpoint = new AiEndpoint
        {
            Name = teamWikiConfig.Name,
            DeploymentName = teamWikiConfig.DeploymentName,
            Key = teamWikiConfig.Key,
            IsSupportImg = teamWikiConfig.IsSupportImg,
            TextMaxToken = teamWikiConfig.TextMaxToken,
            IsSupportFunctionCall = teamWikiConfig.IsSupportFunctionCall,
            Provider = teamWikiConfig.Provider,
            ModelId = teamWikiConfig.ModelId,
            AiFunction = teamWikiConfig.AiFunction,
            EmbeddinMaxToken = teamWikiConfig.EmbeddinMaxToken,
            Endpoint = teamWikiConfig.Endpoint
        };

        // 构建客户端
        var memoryBuilder = new KernelMemoryBuilder().WithSimpleFileStorage(Path.GetTempPath());

        _customKernelMemoryBuilder.ConfigEmbeddingModel(memoryBuilder, aiEndpoint, teamWikiConfig.EmbeddingDimensions, teamWikiConfig.EmbeddingBatchSize, teamWikiConfig.MaxRetries, teamWikiConfig.EmbeddingModelTokenizer);

        var memoryClient = memoryBuilder.WithAzureOpenAITextGeneration(new AzureOpenAIConfig
        {
            // 向量化时用不到文本生成模型，可以乱配置一个
            Deployment = "text-embedding-3-large",
            Endpoint = "https://aaa.openai.azure.com/",
            Auth = AzureOpenAIConfig.AuthTypes.APIKey,
            APIType = AzureOpenAIConfig.APITypes.ChatCompletion,
            APIKey = "00000",
        })
            .WithPostgresMemoryDb(new PostgresConfig
            {
                ConnectionString = _systemOptions.DocumentStore.Database,
            })
            .WithCustomTextPartitioningOptions(
            new TextPartitioningOptions
            {
                MaxTokensPerParagraph = documentTask.MaxTokensPerParagraph,
                OverlappingTokens = documentTask.OverlappingTokens
            })
            .Build();

        var docs = new Microsoft.KernelMemory.Document()
        {
            Id = documentTask.DocumentId.ToString(),
        };

        docs.AddFile(filePath);
        docs.AddTag("teamId", documentTask.TeamId.ToString());
        docs.AddTag("wikiId", documentTask.WikiId.ToString());
        docs.AddTag("fileId", documentFile.FileId.ToString());
        docs.AddTag("fileName", documentFile.FileName);

        try
        {
            var taskId = await memoryClient.ImportDocumentAsync(docs, index: "n" + documentTask.WikiId);
        }
        catch (Exception ex)
        {
            documentTask.State = (int)FileEmbeddingState.Failed;
            documentTask.Message = ex.Message;
            await _databaseContext.SaveChangesAsync();
            throw;
        }

        documentTask.State = (int)FileEmbeddingState.Successful;
        documentTask.Message = "任务处理完成";
        _databaseContext.TeamWikiDocumentTasks.Update(documentTask);
        await _databaseContext.SaveChangesAsync();
    }

    public Task FaildAsync(MessageHeader messageHeader, Exception ex, int retryCount, EmbeddingDocumentEvent message)
    {
        return Task.CompletedTask;
    }

    public Task<ConsumerState> FallbackAsync(MessageHeader messageHeader, EmbeddingDocumentEvent? message, Exception? ex)
    {
        return Task.FromResult(ConsumerState.Ack);
    }
}
