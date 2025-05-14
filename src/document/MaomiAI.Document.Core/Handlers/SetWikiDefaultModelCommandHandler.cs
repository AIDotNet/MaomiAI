// <copyright file="SetWikiDefaultModelCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Document.Shared.Commands;
using MediatR;
using System.Data.Entity;

namespace MaomiAI.Document.Core.Handlers;

/// <summary>
/// 设置知识库默认向量化模型.
/// </summary>
public class SetWikiDefaultModelCommandHandler : IRequestHandler<SetWikiDefaultModelCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetWikiDefaultModelCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public SetWikiDefaultModelCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(SetWikiDefaultModelCommand request, CancellationToken cancellationToken)
    {
        var teamId = await _databaseContext.TeamWikis
                .Where(x => x.Id == request.WikiId)
                .Select(x => x.TeamId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (teamId == default)
        {
            throw new BusinessException("知识库不存在") { StatusCode = 404 };
        }

        var existTeam = await _databaseContext.TeamWikis
            .Where(x => x.Id == request.WikiId)
            .AnyAsync(cancellationToken: cancellationToken);
        if (!existTeam)
        {
            throw new BusinessException("团队不存在") { StatusCode = 404 };
        }

        var config = await _databaseContext.TeamWikiConfigs.Where(x => x.WikiId == request.WikiId).FirstOrDefaultAsync();

        if (config == null)
        {
            config = new TeamWikiConfigEntity
            {
                WikiId = request.WikiId,
                EmbeddingModelId = request.ModelId,
                TeamId = teamId,
            };

            _databaseContext.TeamWikiConfigs.Add(config);
            await _databaseContext.SaveChangesAsync();
        }
        else
        {
            config.EmbeddingModelId = request.ModelId;
            _databaseContext.TeamWikiConfigs.Update(config);
            await _databaseContext.SaveChangesAsync();
        }

        return EmptyCommandResponse.Default;
    }
}