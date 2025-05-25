// <copyright file="EmbeddingDocumentCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.MQ;
using MaomiAI.AiModel.Shared.Models;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Document.Core.Consumers.Events;
using MaomiAI.Document.Shared.Commands.Documents;
using MaomiAI.Document.Shared.Models;
using MaomiAI.Infra;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Handlers.Documents;

/// <summary>
/// 向量化文档.
/// </summary>
public class EmbeddingDocumentCommandHandler : IRequestHandler<EmbeddingocumentCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly SystemOptions _systemOptions;
    private readonly IMessagePublisher _messagePublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmbeddingDocumentCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="systemOptions"></param>
    /// <param name="messagePublisher"></param>
    public EmbeddingDocumentCommandHandler(DatabaseContext databaseContext, SystemOptions systemOptions, IMessagePublisher messagePublisher)
    {
        _databaseContext = databaseContext;
        _systemOptions = systemOptions;
        _messagePublisher = messagePublisher;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(EmbeddingocumentCommand request, CancellationToken cancellationToken)
    {
        var documentTask = await _databaseContext.TeamWikiDocumentTasks
            .AnyAsync(x => x.DocumentId == request.DocumentId && x.State < (int)FileEmbeddingState.Processing);

        if (documentTask == true)
        {
            throw new BusinessException("当前文档已在处理任务，请勿重复添加") { StatusCode = 409 };
        }

        var fileId = await _databaseContext.TeamWikiDocuments.Where(x => x.Id == request.DocumentId)
            .Select(x => x.FileId)
            .FirstOrDefaultAsync();

        if (fileId == default)
        {
            throw new BusinessException("知识库文档不存在") { StatusCode = 404 };
        }

        var teamWikiConfig = await _databaseContext.TeamWikiConfigs
            .Where(x => x.TeamId == request.TeamId && x.WikiId == request.WikiId)
            .Join(_databaseContext.TeamAiModels, a => a.EmbeddingModelId, b => b.Id, (a, b) => new
            {
                b.Id,
                b.AiModelType,
            }).FirstOrDefaultAsync();

        if (teamWikiConfig == null || !AiModelType.Embedding.ToString().Equals(teamWikiConfig.AiModelType, StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessException("知识库未配置向量化模型") { StatusCode = 409 };
        }

        var documentTaskEntity = new TeamWikiDocumentTaskEntity
        {
            DocumentId = request.DocumentId,
            TeamId = request.TeamId,
            WikiId = request.WikiId,
            TaskTag = Guid.NewGuid().ToString(),
            State = (int)FileEmbeddingState.Wait,
            ModelId = teamWikiConfig.Id,
            FileId = fileId,
            MaxTokensPerParagraph = request.MaxTokensPerParagraph,
            OverlappingTokens = request.OverlappingTokens,
            Tokenizer = request.Tokenizer,
            Message = "任务已创建"
        };

        await _databaseContext.TeamWikiDocumentTasks.AddAsync(documentTaskEntity, cancellationToken);
        await _databaseContext.TeamWikiConfigs.ExecuteUpdateAsync(x => x.SetProperty(x => x.IsLock, true), cancellationToken: cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        // 后台处理
        var embeddingDocumentEvent = new EmbeddingDocumentEvent
        {
            DocumentId = request.DocumentId,
            TeamId = request.TeamId,
            WikiId = request.WikiId,
            TaskId = documentTaskEntity.Id,
        };

        await _messagePublisher.PublishAsync(model: embeddingDocumentEvent);

        return EmptyCommandResponse.Default;
    }
}
