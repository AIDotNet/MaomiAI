// <copyright file="ImportMcpServerCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Exceptions;
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

    /// <inheritdoc/>
    public async Task<IdResponse> Handle(ImportMcpServerCommand request, CancellationToken cancellationToken)
    {
        // 后续抽到一个方法命令中
        Dictionary<string, string> headers = default!;
        Dictionary<string, string> queries = default!;
        try
        {
            headers = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(request.Header);
            queries = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(request.Query);
        }
        catch (Exception ex)
        {
            _ = ex;
            throw new BusinessException("Header 或 Query 格式不正确");
        }

        var defaultOptions = new McpClientOptions
        {
            ClientInfo = new() { Name = "MaomiAI", Version = "1.0.0" }
        };

        var uriBuilder = new UriBuilder(request.ServerUrl);
        if (queries != null && queries.Count > 0)
        {
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var kv in queries)
            {
                query[kv.Key] = kv.Value;
            }

            uriBuilder.Query = query.ToString();
        }

        var serverUrl = uriBuilder.Uri;
        var defaultConfig = new SseClientTransportOptions
        {
            Endpoint = serverUrl,
            Name = request.Name,
            AdditionalHeaders = headers ?? new Dictionary<string, string>(),
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

        var pluginGroup = new TeamPluginGroupEntity
        {
            Server = request.ServerUrl,
            Name = request.Name,
            Type = (int)PluginType.Mcp,
            TeamId = request.TeamId,
            Header = request.Header,
            Query = request.Query,
            Description = request.Description,
        };

        await _databaseContext.TeamPluginGroups.AddAsync(pluginGroup, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        foreach (var item in teamPluginEntities)
        {
            item.GroupId = pluginGroup.Id;
            item.Path = string.Empty;
        }

        await _databaseContext.TeamPlugins.AddRangeAsync(teamPluginEntities, cancellationToken);
        await _databaseContext.SaveChangesAsync();

        transactionScope.Complete();

        return new IdResponse { Id = pluginGroup.Id };
    }
}
