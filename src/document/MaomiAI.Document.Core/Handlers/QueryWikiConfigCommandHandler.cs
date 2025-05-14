// <copyright file="QueryWikiConfigCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Document.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Handlers;

public class QueryWikiConfigCommandHandler : IRequestHandler<QueryWikiConfigCommand, QueryWikiConfigCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiConfigCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public QueryWikiConfigCommandHandler(DatabaseContext databaseContext, IMediator mediator)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
    }

    public async Task<QueryWikiConfigCommandResponse> Handle(QueryWikiConfigCommand request, CancellationToken cancellationToken)
    {
        var result = await _databaseContext.TeamWikiConfigs.Where(x => x.WikiId == request.WikiId)
            .Select(x => new QueryWikiConfigCommandResponse
            {
                EmbeddingDimensions = x.EmbeddingDimensions,
                EmbeddingModelId = x.EmbeddingModelId,
                EmbeddingModelTokenizer = x.EmbeddingModelTokenizer,
                CreateTime = x.CreateTime,
                UpdateTime = x.UpdateTime,
                CreateUserId = x.CreateUserId,
                UpdateUserId = x.UpdateUserId,
                IsLock = x.IsLock,
                EmbeddingBatchSize = x.EmbeddingBatchSize,
                MaxRetries = x.MaxRetries,
                WikiId = x.WikiId,
            }).FirstOrDefaultAsync();

        if (result == null)
        {
            throw new BusinessException("获取知识库配置失败，请联系管理员") { StatusCode = 500 };
        }

        await _mediator.Send(new FillUserInfoCommand { Items = new List<AuditsInfo> { result } });
        return result;
    }
}
