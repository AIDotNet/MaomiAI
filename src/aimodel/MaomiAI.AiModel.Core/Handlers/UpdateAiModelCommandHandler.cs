// <copyright file="UpdateAiModelCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.AiModel.Shared.Commands;
using MaomiAI.Database;
using MaomiAI.Infra.Helpers;
using MaomiAI.Infra.Service;
using MaomiAI.Infra.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.AiModel.Core.Handlers;

/// <summary>
/// 更新模型.
/// </summary>
public class UpdateAiModelCommandHandler : IRequestHandler<UpdateAiModelCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly IRsaProvider _rsaProvider;
    private readonly IAESProvider _aesProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAiModelCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="rsaProvider"></param>
    /// <param name="aesProvider"></param>
    public UpdateAiModelCommandHandler(DatabaseContext dbContext, IRsaProvider rsaProvider, IAESProvider aesProvider)
    {
        _dbContext = dbContext;
        _rsaProvider = rsaProvider;
        _aesProvider = aesProvider;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(UpdateAiModelCommand request, CancellationToken cancellationToken)
    {
        var aiModel = await _dbContext.TeamAiModels
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TeamId == request.TeamId && x.Id == request.ModelId, cancellationToken);

        if (aiModel == null)
        {
            throw new BusinessException("模型不存在") { StatusCode = 404 };
        }

        if (aiModel.Name != request.Endpoint.Name)
        {
            var existModel = await _dbContext.TeamAiModels
                .AsNoTracking()
                .AnyAsync(x => x.TeamId == request.TeamId && x.Name == request.Endpoint.Name, cancellationToken);

            if (existModel)
            {
                throw new BusinessException("已存在同名模型") { StatusCode = 409 };
            }
        }

        if (!string.IsNullOrEmpty(request.Endpoint.Key) && request.Endpoint.Key.Distinct().Count() > 1)
        {
            try
            {
                string skKey = _rsaProvider.Decrypt(request.Endpoint.Key);
                skKey = _aesProvider.Encrypt(skKey);
                aiModel.Key = skKey;
            }
            catch (Exception ex)
            {
                _ = ex;
                throw new BusinessException("key未正确加密") { StatusCode = 400 };
            }
        }

        aiModel.Name = request.Endpoint.Name;
        aiModel.DeploymentName = request.Endpoint.DeploymentName;
        aiModel.DisplayName = request.Endpoint.DisplayName;
        aiModel.AiModelType = request.Endpoint.AiModelType.ToString();
        aiModel.AiProvider = request.Endpoint.Provider.ToString();
        aiModel.ContextWindowTokens = request.Endpoint.ContextWindowTokens;
        aiModel.Endpoint = request.Endpoint.Endpoint;
        aiModel.MaxDimension = request.Endpoint.MaxDimension;
        aiModel.TextOutput = request.Endpoint.TextOutput;
        aiModel.FunctionCall = request.Endpoint.Abilities?.FunctionCall ?? false;
        aiModel.Files = request.Endpoint.Abilities?.Files ?? false;
        aiModel.ImageOutput = request.Endpoint.Abilities?.ImageOutput ?? false;
        aiModel.Vision = request.Endpoint.Abilities?.Vision ?? false;

        _dbContext.TeamAiModels.Update(aiModel);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return EmptyCommandResponse.Default;
    }
}