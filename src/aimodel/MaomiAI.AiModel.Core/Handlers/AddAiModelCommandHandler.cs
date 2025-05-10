// <copyright file="AddAiModelCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Commands;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Helpers;
using MaomiAI.Infra.Service;
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
    private readonly IAESProvider _aesProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddAiModelCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="rsaProvider"></param>
    /// <param name="aesProvider"></param>
    public AddAiModelCommandHandler(DatabaseContext dbContext, IRsaProvider rsaProvider, IAESProvider aesProvider)
    {
        _dbContext = dbContext;
        _rsaProvider = rsaProvider;
        _aesProvider = aesProvider;
    }

    /// <inheritdoc/>
    public async Task<IdResponse> Handle(AddAiModelCommand request, CancellationToken cancellationToken)
    {
        var existModel = await _dbContext.TeamAiModels
            .AsNoTracking()
            .AnyAsync(x => x.TeamId == request.TeamId && x.Name == request.Endpoint.Name, cancellationToken);

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

        // 重新使用 AES加密
        skKey = _aesProvider.Encrypt(skKey);

        var aiModel = new TeamAiModelEntity
        {
            TeamId = request.TeamId,
            Endpoint = request.Endpoint.Enpoint,
            DeploymentName = request.Endpoint.DeploymentName,
            ModeId = request.Endpoint.ModelId,
            AiModelFunction = (int)EnumHelper.ComposeFlags(request.Endpoint.AiFunction),
            AiProvider = request.Endpoint.Provider.ToString(),
            Key = skKey,
            IsSupportImg = request.Endpoint.IsSupportImg,
            IsSupportFunctionCall = request.Endpoint.IsSupportFunctionCall,
            Name = request.Endpoint.Name,
        };

        await _dbContext.AddAsync(aiModel, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new IdResponse
        {
            Id = aiModel.Id,
        };
    }
}
