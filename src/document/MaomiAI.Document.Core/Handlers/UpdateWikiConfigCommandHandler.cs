// <copyright file="QueryWikiConfigCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Queries;
using MaomiAI.Document.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Handlers;

public class UpdateWikiConfigCommandHandler : IRequestHandler<UpdateWikiConfigCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateWikiConfigCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public UpdateWikiConfigCommandHandler(DatabaseContext databaseContext, IMediator mediator)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
    }

    public async Task<EmptyCommandResponse> Handle(UpdateWikiConfigCommand request, CancellationToken cancellationToken)
    {
        var result = await _databaseContext.TeamWikiConfigs.Where(x => x.WikiId == request.WikiId).FirstOrDefaultAsync();

        if (result == null)
        {
            throw new BusinessException("更新知识库配置失败，请联系管理员") { StatusCode = 500 };
        }

        result.EmbeddingDimensions = request.EmbeddingDimensions;
        result.EmbeddingModelId = request.EmbeddingModelId;
        result.EmbeddingModelTokenizer = request.EmbeddingModelTokenizer;
        result.EmbeddingBatchSize = request.EmbeddingBatchSize;
        result.MaxRetries = request.MaxRetries;

        _databaseContext.TeamWikiConfigs.Update(result);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return EmptyCommandResponse.Default;
    }
}
