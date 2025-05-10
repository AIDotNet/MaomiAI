// <copyright file="QueryAiModelEndpointListCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;
using MaomiAI.AiModel.Shared.Queries;
using MaomiAI.AiModel.Shared.Queries.Respones;
using MaomiAI.Database;
using MaomiAI.Infra.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.AiModel.Core.Queries;

/// <summary>
/// 查询供应商下已配置的模型列表.
/// </summary>
public class QueryAiModelEndpointListCommandHandler : IRequestHandler<QueryAiModelListCommand, QueryAiModelListCommandResponse>
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryAiModelEndpointListCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public QueryAiModelEndpointListCommandHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<QueryAiModelListCommandResponse> Handle(QueryAiModelListCommand request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.TeamAiModels
                .Where(x => x.TeamId == request.TeamId && x.AiProvider == request.Provider.ToString())
                .Select(x => new AiNotKeyEndpoint
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsSupportFunctionCall = x.IsSupportFunctionCall,
                    IsSupportImg = x.IsSupportImg,
                    DeploymentName = x.DeploymentName,
                    Enpoint = x.Endpoint,
                    AiFunction = EnumHelper.DecomposeFlags<AiModelFunction>(x.AiModelFunction),
                    ModelId = x.ModeId,
                    Provider = x.AiProvider
                }).ToArrayAsync();

        return new QueryAiModelListCommandResponse
        {
            AiModels = list
        };
    }
}