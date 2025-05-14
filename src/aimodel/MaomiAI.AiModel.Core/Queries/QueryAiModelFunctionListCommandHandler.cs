// <copyright file="QueryAiModelFunctionListCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Models;
using MaomiAI.AiModel.Shared.Queries;
using MaomiAI.Database;
using MaomiAI.Infra.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.AiModel.Core.Queries;

public class QueryAiModelFunctionListCommandHandler : IRequestHandler<QueryAiModelFunctionListCommand, QueryAiModelFunctionListCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    public QueryAiModelFunctionListCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<QueryAiModelFunctionListCommandResponse> Handle(QueryAiModelFunctionListCommand request, CancellationToken cancellationToken)
    {
        var enumValue = (int)request.AiModelFunction;
        var result = await _databaseContext.TeamAiModels
            .Where(x => x.TeamId == request.TeamId && x.AiModelFunction == (x.AiModelFunction & enumValue))
                .Select(x => new AiNotKeyEndpoint
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsSupportFunctionCall = x.IsSupportFunctionCall,
                    IsSupportImg = x.IsSupportImg,
                    DeploymentName = x.DeploymentName,
                    endpoint = x.Endpoint,
                    AiFunction = EnumHelper.DecomposeFlags<AiModelFunction>(x.AiModelFunction),
                    ModelId = x.ModeId,
                    Provider = x.AiProvider,
                    EmbeddinMaxToken = x.EmbeddinMaxToken,
                    TextMaxToken = x.TextMaxToken
                }).ToArrayAsync();

        return new QueryAiModelFunctionListCommandResponse
        {
            AiModels = result
        };
    }
}
