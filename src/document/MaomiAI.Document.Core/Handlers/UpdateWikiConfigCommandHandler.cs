// <copyright file="UpdateWikiConfigCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Document.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Handlers;

/// <summary>
/// 更新知识库配置.
/// </summary>
public class UpdateWikiConfigCommandHandler : IRequestHandler<UpdateWikiConfigCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateWikiConfigCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="mediator"></param>
    public UpdateWikiConfigCommandHandler(DatabaseContext databaseContext, IMediator mediator)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(UpdateWikiConfigCommand request, CancellationToken cancellationToken)
    {
        var result = await _databaseContext.TeamWikiConfigs.Where(x => x.WikiId == request.WikiId).FirstOrDefaultAsync();

        if (result == null)
        {
            throw new BusinessException("更新知识库配置失败，请联系管理员") { StatusCode = 500 };
        }

        if (result.IsLock)
        {
            throw new BusinessException("知识库已进行文档处理，禁止改动配置") { StatusCode = 409 };
        }

        result.EmbeddingDimensions = request.Config.EmbeddingDimensions;
        result.EmbeddingModelId = request.Config.EmbeddingModelId;
        result.EmbeddingModelTokenizer = request.Config.EmbeddingModelTokenizer;
        result.EmbeddingBatchSize = request.Config.EmbeddingBatchSize;
        result.MaxRetries = request.Config.MaxRetries;

        _databaseContext.TeamWikiConfigs.Update(result);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return EmptyCommandResponse.Default;
    }
}
