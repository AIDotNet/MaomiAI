// <copyright file="UpdateAiModelCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.AiModel.Shared.Commands;
using MaomiAI.Database;
using MaomiAI.Infra.Service;
using MaomiAI.Infra.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.AiModel.Core.Handlers;

/// <summary>
/// 设置默认模型.
/// </summary>
public class SetDefaultAiModelCommandHandler : IRequestHandler<SetDefaultAiModelCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetDefaultAiModelCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public SetDefaultAiModelCommandHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(SetDefaultAiModelCommand request, CancellationToken cancellationToken)
    {
        var aiModel = await _dbContext.TeamAiModels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TeamId == request.TeamId && x.Id == request.ModelId, cancellationToken);

        if (aiModel == null)
        {
            throw new BusinessException("模型不存在") { StatusCode = 404 };
        }

        var defaultModel = await _dbContext.TeamDefaultAiModels
            .FirstOrDefaultAsync(x => x.TeamId == request.TeamId && x.Function == (int)request.AiFunction, cancellationToken);

        if (defaultModel == null)
        {
            defaultModel = new Database.Entities.TeamDefaultAiModelEntity
            {
                TeamId = request.TeamId,
                ModelId = request.ModelId,
                Function = (int)request.AiFunction
            };

            await _dbContext.AddAsync(defaultModel, cancellationToken);
        }
        else
        {
            defaultModel.ModelId = request.ModelId;
            _dbContext.TeamDefaultAiModels.Update(defaultModel);
        }

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new BusinessException("更新冲突，请重试") { StatusCode = 409 };
        }

        return EmptyCommandResponse.Default;
    }
}