// <copyright file="AddAiModelCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Commands;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.AiModel.Core.Handlers;

/// <summary>
/// 添加模型.
/// </summary>
public class AddAiModelCommandHandler : IRequestHandler<AddAiModelCommand, IdResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly IRsaProvider _rsaProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddAiModelCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="rsaProvider"></param>
    public AddAiModelCommandHandler(DatabaseContext dbContext, IRsaProvider rsaProvider)
    {
        _dbContext = dbContext;
        _rsaProvider = rsaProvider;
    }

    /// <inheritdoc/>
    public async Task<IdResponse> Handle(AddAiModelCommand request, CancellationToken cancellationToken)
    {
        var existModel = await _dbContext.TeamAiModels
            .AsNoTracking()
            .AnyAsync(x => x.TeamId == request.TeamId && x.DisplayName == request.Endpoint.DisplayName, cancellationToken);

        if (existModel)
        {
            throw new BusinessException("已存在同名模型") { StatusCode = 409 };
        }

        string skKey = string.Empty;
        try
        {
            skKey = _rsaProvider.Decrypt(request.Endpoint.Key);
        }
        catch (Exception ex)
        {
            _ = ex;
            throw new BusinessException("key未正确加密") { StatusCode = 400 };
        }

        var aiModel = new TeamAiModelEntity
        {
            TeamId = request.TeamId,
            Name = request.Endpoint.Name,
            DeploymentName = request.Endpoint.DeploymentName,
            DisplayName = request.Endpoint.DisplayName,
            AiModelType = request.Endpoint.AiModelType.ToString(),
            AiProvider = request.Endpoint.Provider.ToString(),
            ContextWindowTokens = request.Endpoint.ContextWindowTokens,
            Endpoint = request.Endpoint.Endpoint,
            Key = skKey,
            MaxDimension = request.Endpoint.MaxDimension,
            TextOutput = request.Endpoint.TextOutput,
            FunctionCall = request.Endpoint.Abilities.FunctionCall ?? false,
            Files = request.Endpoint.Abilities.Files ?? false,
            ImageOutput = request.Endpoint.Abilities.ImageOutput ?? false,
            Vision = request.Endpoint.Abilities.Vision ?? false,
        };

        await _dbContext.AddAsync(aiModel, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new IdResponse
        {
            Id = aiModel.Id,
        };
    }
}
