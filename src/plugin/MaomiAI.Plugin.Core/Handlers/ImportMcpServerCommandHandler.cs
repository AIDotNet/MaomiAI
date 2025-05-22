// <copyright file="ImportMcpServerCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Models;
using MaomiAI.Plugin.Shared.Commands;
using MaomiAI.Plugin.Shared.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;
using System.Transactions;

namespace MaomiAI.Plugin.Core.Handlers;

public class ImportMcpServerCommandHandler : IRequestHandler<ImportMcpServerCommand, IdResponse>
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly DatabaseContext _databaseContext;

    public ImportMcpServerCommandHandler(ILoggerFactory loggerFactory, DatabaseContext databaseContext)
    {
        _loggerFactory = loggerFactory;
        _databaseContext = databaseContext;
    }

    public async Task<IdResponse> Handle(ImportMcpServerCommand request, CancellationToken cancellationToken)
    {
        var defaultOptions = new McpClientOptions
        {
            ClientInfo = new() { Name = "MaomiAI", Version = "1.0.0" }
        };

        var defaultConfig = new SseClientTransportOptions
        {
            Endpoint = new Uri(request.ServerUrl),
            Name = request.Name,
        };

        await using var sseTransport = new SseClientTransport(defaultConfig);
        await using var client = await McpClientFactory.CreateAsync(
         sseTransport,
         defaultOptions,
         loggerFactory: _loggerFactory);

        var tools = await client.ListToolsAsync();

        List<TeamPluginEntity> teamPluginEntities = new List<TeamPluginEntity>();
        foreach (var tool in tools)
        {
            var pluginEntity = new TeamPluginEntity
            {
                Id = Guid.NewGuid(),
                TeamId = request.TeamId,
                Name = tool.Name,
                Summary = tool.Description
            };

            teamPluginEntities.Add(pluginEntity);
        }

        using TransactionScope transactionScope = new TransactionScope(
            scopeOption: TransactionScopeOption.Required,
            asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled,
            transactionOptions: new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead });

        // 自动生成新的分组
        var pluginGroup = new TeamPluginGroupEntity
        {
            Server = request.ServerUrl,
            Name = DateTimeOffset.Now.Ticks.ToString(),
            Type = (int)PluginType.Mcp,
            TeamId = request.TeamId,
            Header = "{}",
            Description = request.Description
        };

        await _databaseContext.TeamPluginGroups.AddAsync(pluginGroup, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        foreach (var item in teamPluginEntities)
        {
            item.GroupId = pluginGroup.Id;
        }

        await _databaseContext.TeamPlugins.AddRangeAsync(teamPluginEntities, cancellationToken);
        await _databaseContext.SaveChangesAsync();

        transactionScope.Complete();

        return new IdResponse { Id = pluginGroup.Id };
    }
}
