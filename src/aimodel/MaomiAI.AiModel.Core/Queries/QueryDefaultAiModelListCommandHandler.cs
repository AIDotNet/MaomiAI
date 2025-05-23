﻿// <copyright file="QueryDefaultAiModelListCommandHandler.cs" company="MaomiAI">
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
/// 查询供应商默认模型列表.
/// </summary>
public class QueryDefaultAiModelListCommandHandler : IRequestHandler<QueryDefaultAiModelListCommand, QueryDefaultAiModelListResponse>
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryDefaultAiModelListCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public QueryDefaultAiModelListCommandHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<QueryDefaultAiModelListResponse> Handle(QueryDefaultAiModelListCommand request, CancellationToken cancellationToken)
    {
        return new QueryDefaultAiModelListResponse
        {
            AiModels = null
        };
    }
}