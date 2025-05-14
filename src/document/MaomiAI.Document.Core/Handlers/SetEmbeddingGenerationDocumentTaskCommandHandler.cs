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

namespace MaomiAI.Document.Core.Handlers;

public class SetEmbeddingGenerationDocumentTaskCommandHandler : IRequestHandler<SetEmbeddingGenerationDocumentTaskCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    public SetEmbeddingGenerationDocumentTaskCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<EmptyCommandResponse> Handle(SetEmbeddingGenerationDocumentTaskCommand request, CancellationToken cancellationToken)
    {
        // 存储参数到数据库
        // 发送任务到消息队列或后台
        throw new NotImplementedException();
    }
}

public class EmbeddingDocumentCommandHandler : IRequestHandler<EmbeddingocumentCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly SystemOptions _systemOptions;
    private readonly IMessagePublisher _messagePublisher;

    public async Task<EmptyCommandResponse> Handle(EmbeddingocumentCommand request, CancellationToken cancellationToken)
    {
        var documentTask = await _databaseContext.TeamWikiDocumentTasks
            .AnyAsync(x => x.DocumentId == request.DocumentId && x.State < (int)FileEmbeddingState.Processing);

        if (documentTask == true)
        {
            throw new BusinessException("当前文档已存在任务，请勿重复操作") { StatusCode = 409 };
        }

        var teamWikiConfig = await _databaseContext.TeamWikiConfigs
            .Where(x => x.TeamId == request.TeamId && x.WikiId == request.WikiId)
            .Join(_databaseContext.TeamAiModels, a => a.EmbeddingModelId, b => b.Id, (a, b) => new
            {
                b.Id,
                b.AiModelFunction
            }).FirstOrDefaultAsync();

        if (teamWikiConfig == null || !((AiModelFunction)teamWikiConfig.AiModelFunction).HasFlag(AiModelFunction.TextEmbeddingGeneration))
        {
            throw new BusinessException("知识库未配置向量化模型");
        }

        var documentTaskEntity = new TeamWikiDocumentTaskEntity
        {
            DocumentId = request.DocumentId,
            TeamId = request.TeamId,
            WikiId = request.WikiId,
            TaskId = Guid.NewGuid().ToString(),
            State = (int)FileEmbeddingState.Wait,
            ModelId = teamWikiConfig.Id,
            MaxTokensPerParagraph = request.MaxTokensPerParagraph,
            OverlappingTokens = request.OverlappingTokens,
            Tokenizer = request.Tokenizer,
            Message = "任务已创建"
        };

        await _databaseContext.TeamWikiDocumentTasks.AddAsync(documentTaskEntity, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        var embeddingDocumentEvent = new EmbeddingDocumentEvent
        {
            DocumentId = request.DocumentId,
            TeamId = request.TeamId,
            WikiId = request.WikiId,
            TaskId = documentTaskEntity.TaskId,
        };

        await _messagePublisher.PublishAsync(model: embeddingDocumentEvent);

        return EmptyCommandResponse.Default;
    }
}
