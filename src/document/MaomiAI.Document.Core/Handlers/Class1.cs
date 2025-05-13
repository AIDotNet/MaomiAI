//// <copyright file="Class1.cs" company="MaomiAI">
//// Copyright (c) MaomiAI. All rights reserved.
//// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//// Github link: https://github.com/AIDotNet/MaomiAI
//// </copyright>

//using MaomiAI.Database;
//using MaomiAI.Document.Shared.Models;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.KernelMemory;
//using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

//namespace MaomiAI.Document.Core.Handlers;

//public class SetEmbeddingGenerationDocumentTaskCommandHandler : IRequestHandler<SetEmbeddingGenerationDocumentTaskCommand, EmptyCommandResponse>
//{
//    private readonly DatabaseContext _databaseContext;

//    public SetEmbeddingGenerationDocumentTaskCommandHandler(DatabaseContext databaseContext)
//    {
//        _databaseContext = databaseContext;
//    }

//    public Task<EmptyCommandResponse> Handle(SetEmbeddingGenerationDocumentTaskCommand request, CancellationToken cancellationToken)
//    {
//        // 存储参数到数据库
//        // 发送任务到消息队列或后台
//        throw new NotImplementedException();
//    }
//}

//public class EmbeddingGenerationDocumentCommandHandler : IRequestHandler<EmbeddingGenerationDocumentCommand, EmptyCommandResponse>
//{
//    private readonly DatabaseContext _databaseContext;

//    public EmbeddingGenerationDocumentCommandHandler(DatabaseContext databaseContext)
//    {
//        _databaseContext = databaseContext;
//    }

//    public async Task<EmptyCommandResponse> Handle(EmbeddingGenerationDocumentCommand request, CancellationToken cancellationToken)
//    {
//        // 读取数据库任务信息

//        var documentTask = await _databaseContext.TeamWikiDocumentTasks.FirstOrDefaultAsync(x => x.DocumentId == request.DocumentId && x.TaskId == request.TaskId);
//        if (documentTask == null || documentTask.State != (int)FileEmbeddingGenerationState.None)
//        {
//            return EmptyCommandResponse.Default;
//        }

//        // 读取要处理的文件
//        // 启动 km
//        // 处理文件
//        // 执行结果刷新到数据库

//        var memory = new KernelMemoryBuilder()
//            .WithSimpleFileStorage(Path.GetTempPath())
//            .WithAzureOpenAITextEmbeddingGeneration(new AzureOpenAIConfig
//            {
//                Deployment = aiModel.ModelId,
//                Endpoint = aiModel.Endpoint!,
//                Auth = AzureOpenAIConfig.AuthTypes.APIKey,
//                APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
//                APIKey = aiModel.Key
//            })
//            .WithOpenAITextEmbeddingGeneration()
//            // 向量化时用不到文本生成模型，可以乱配置一个
//            .WithAzureOpenAITextGeneration(new AzureOpenAIConfig
//            {
//                Deployment = aiModel.ModelId,
//                Endpoint = aiModel.Endpoint,
//                Auth = AzureOpenAIConfig.AuthTypes.APIKey,
//                APIKey = aiModel.Key,
//                APIType = AzureOpenAIConfig.APITypes.ChatCompletion
//            })
//            .WithPostgresMemoryDb(new PostgresConfig
//            {
//                ConnectionString = settings.PostgresMemoryDb
//            }).WithCustomTextGenerator(new CustomTextGeneratorConfig
//            {
//                Model = aiModel.ModelId,
//                Endpoint = aiModel.Endpoint,
//                Auth = AzureOpenAIConfig.AuthTypes.APIKey,
//                APIKey = aiModel.Key,
//                APIType = AzureOpenAIConfig.APITypes.ChatCompletion
//            })

//            .Build();
//    }

//    // 配置序列化模型
//}
