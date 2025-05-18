// <copyright file="SearchWikiDocumentTextCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;
using MaomiAI.Database;
using MaomiAI.Document.Core.Handlers.Responses;
using MaomiAI.Document.Core.Services;
using MaomiAI.Document.Shared.Models;
using MaomiAI.Document.Shared.Queries.Documents;
using MaomiAI.Infra;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.KernelMemory;

namespace MaomiAI.Document.Core.Queries.Documents;

/// <summary>
/// 搜索知识库文本.
/// </summary>
public class SearchWikiDocumentTextCommandHandler : IRequestHandler<SearchWikiDocumentTextCommand, SearchWikiDocumentTextCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly CustomKernelMemoryBuilder _customKernelMemoryBuilder;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="SearchWikiDocumentTextCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="customKernelMemoryBuilder"></param>
    /// <param name="systemOptions"></param>
    public SearchWikiDocumentTextCommandHandler(DatabaseContext databaseContext, CustomKernelMemoryBuilder customKernelMemoryBuilder, SystemOptions systemOptions)
    {
        _databaseContext = databaseContext;
        _customKernelMemoryBuilder = customKernelMemoryBuilder;
        _systemOptions = systemOptions;
    }

    /// <inheritdoc/>
    public async Task<SearchWikiDocumentTextCommandResponse> Handle(SearchWikiDocumentTextCommand request, CancellationToken cancellationToken)
    {
        var document = await _databaseContext.TeamWikiDocuments.FirstOrDefaultAsync(x => x.Id == request.DocumentId);
        if (document == null)
        {
            throw new BusinessException("文档不存在") { StatusCode = 404 };
        }

        var teamWikiAiConfig = await _databaseContext.TeamWikiConfigs
        .Where(x => x.TeamId == document.TeamId && x.WikiId == document.WikiId)
        .Join(_databaseContext.TeamAiModels, a => a.EmbeddingModelId, b => b.Id, (a, x) => new
        {
            WikiConfig = new WikiConfig
            {
                EmbeddingDimensions = a.EmbeddingDimensions,
                EmbeddingBatchSize = a.EmbeddingBatchSize,
                MaxRetries = a.MaxRetries,
                EmbeddingModelTokenizer = a.EmbeddingModelTokenizer,
            },
            AiEndpoint = new AiEndpoint
            {
                Name = x.Name,
                DeploymentName = x.DeploymentName,
                DisplayName = x.DisplayName,
                AiModelType = Enum.Parse<AiModelType>(x.AiModelType, true),
                Provider = Enum.Parse<AiProvider>(x.AiProvider, true),
                ContextWindowTokens = x.ContextWindowTokens,
                Endpoint = x.Endpoint,
                Key = x.Key,
                Abilities = new ModelAbilities
                {
                    Files = x.Files,
                    FunctionCall = x.FunctionCall,
                    ImageOutput = x.ImageOutput,
                    Vision = x.Vision,
                },
                MaxDimension = x.MaxDimension,
                TextOutput = x.TextOutput
            }
        }).FirstOrDefaultAsync();

        if (teamWikiAiConfig == null)
        {
            throw new BusinessException("团队配置错误") { StatusCode = 500 };
        }

        // 构建客户端
        var memoryBuilder = new KernelMemoryBuilder().WithSimpleFileStorage(Path.GetTempPath());

        _customKernelMemoryBuilder.ConfigEmbeddingModel(memoryBuilder, teamWikiAiConfig.AiEndpoint, teamWikiAiConfig.WikiConfig);

        var memoryClient = memoryBuilder.WithoutTextGenerator()
            .WithPostgresMemoryDb(new PostgresConfig
            {
                ConnectionString = _systemOptions.DocumentStore.Database,
            })
            .Build();

        var query = string.IsNullOrEmpty(request.Query) ? string.Empty : request.Query;
        var searchResult = await memoryClient.SearchAsync(query: query, index: "n" + document.WikiId, limit: 5, filter: new MemoryFilter
            {
                { "fileId", document.FileId.ToString() },
            });

        if (searchResult == null)
        {
            return new SearchWikiDocumentTextCommandResponse
            {
                SearchResult = new SearchResult()
            };
        }

        return new SearchWikiDocumentTextCommandResponse
        {
            SearchResult = searchResult,
        };
    }
}