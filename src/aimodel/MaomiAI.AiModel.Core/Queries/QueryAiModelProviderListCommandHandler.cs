﻿// <copyright file="QueryAiModelProviderListCommandHandler.cs" company="MaomiAI">
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
using System.Reflection;
using System.Text.Json.Serialization;

namespace MaomiAI.AiModel.Core.Queries;

/// <summary>
/// 查询 ai 供应商列表和模型数量.
/// </summary>
public class QueryAiModelProviderListCommandHandler : IRequestHandler<QueryAiModelProviderListCommand, QueryAiModelProviderListResponse>
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryAiModelProviderListCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public QueryAiModelProviderListCommandHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<QueryAiModelProviderListResponse> Handle(QueryAiModelProviderListCommand request, CancellationToken cancellationToken)
    {
        var providers = new List<QueryAiModelProviderCount>();

        var list = await _dbContext.TeamAiModels
            .Where(x => x.TeamId == request.TeamId)
            .GroupBy(x => x.AiProvider)
            .Select(x => new QueryAiModelProviderCount
            {
                Provider = x.Key,
                Count = x.Count()
            })
            .ToListAsync(cancellationToken);

        foreach (var item in typeof(AiProvider).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            string name = item.Name;
            var jsonName = item.GetCustomAttribute<JsonPropertyNameAttribute>();
            if (jsonName != null)
            {
                name = jsonName.Name;
            }

            providers.Add(new QueryAiModelProviderCount
            {
                Provider = name,
                Count = list.FirstOrDefault(x => x.Provider.Equals(name, StringComparison.OrdinalIgnoreCase))?.Count ?? 0
            });
        }

        return new QueryAiModelProviderListResponse
        {
            Providers = providers
        };
    }
}