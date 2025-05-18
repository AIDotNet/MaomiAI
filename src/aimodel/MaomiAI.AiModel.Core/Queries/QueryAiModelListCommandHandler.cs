// <copyright file="QueryAiModelListCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;
using MaomiAI.AiModel.Shared.Queries;
using MaomiAI.AiModel.Shared.Queries.Respones;
using MaomiAI.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.AiModel.Core.Queries;

/// <summary>
/// 查询模型列表.
/// </summary>
public class QueryAiModelListCommandHandler : IRequestHandler<QueryAiModelListCommand, QueryAiModelListCommandResponse>
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryAiModelListCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public QueryAiModelListCommandHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<QueryAiModelListCommandResponse> Handle(QueryAiModelListCommand request, CancellationToken cancellationToken)
    {
        var query = _dbContext.TeamAiModels
                .Where(x => x.TeamId == request.TeamId);

        if (!string.IsNullOrEmpty(request.Provider?.ToString()))
        {
            query = query.Where(x => x.AiProvider == request.Provider.ToString());
        }

        if (request.AiModelType != null)
        {
            query = query.Where(x => x.AiModelType == request.AiModelType.ToString());
        }

        var list = await query
                .Select(x => new AiNotKeyEndpoint
                {
                    Id = x.Id,
                    Name = x.Name,
                    DeploymentName = x.DeploymentName,
                    DisplayName = x.DisplayName,
                    AiModelType = Enum.Parse<AiModelType>(x.AiModelType, true),
                    Provider = Enum.Parse<AiProvider>(x.AiProvider, true),
                    ContextWindowTokens = x.ContextWindowTokens,
                    Endpoint = x.Endpoint,
                    Abilities = new ModelAbilities
                    {
                        Files = x.Files,
                        FunctionCall = x.FunctionCall,
                        ImageOutput = x.ImageOutput,
                        Vision = x.Vision,
                    },
                    MaxDimension = x.MaxDimension,
                    TextOutput = x.TextOutput
                }).ToArrayAsync(cancellationToken: cancellationToken);

        return new QueryAiModelListCommandResponse
        {
            AiModels = list
        };
    }
}