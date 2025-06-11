// <copyright file="PreUploadOpenApiFileCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Plugin.Shared.Commands;
using MaomiAI.Plugin.Shared.Commands.Responses;
using MaomiAI.Plugin.Shared.Models;
using MaomiAI.Store.Commands;
using MaomiAI.Store.Enums;
using MaomiAI.Store.Queries;
using MaomiAI.Team.Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Writers;
using System.Transactions;

namespace MaomiAI.Plugin.Core.Commands;

/// <summary>
/// 完成 openapi 文件上传，并拆解生成到数据库.
/// </summary>
public class ComplateOpenApiFileCommandHandler : IRequestHandler<ComplateOpenApiFileCommand, IdResponse>
{
    private readonly IMediator _mediator;
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComplateOpenApiFileCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="databaseContext"></param>
    public ComplateOpenApiFileCommandHandler(IMediator mediator, DatabaseContext databaseContext)
    {
        _mediator = mediator;
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<IdResponse> Handle(ComplateOpenApiFileCommand request, CancellationToken cancellationToken)
    {
        var fileEntity = await _databaseContext.Files.Where(x => x.Id == request.FileId).FirstOrDefaultAsync();
        if (fileEntity == null)
        {
            throw new BusinessException("文件不存在") { StatusCode = 404 };
        }

        await _mediator.Send(new ComplateFileUploadCommand { FileId = request.FileId, IsSuccess = true });

        // 拉取完整的 openapi 文件
        var tempFile = FileStoreHelper.GetTempFilePath(Path.GetExtension(fileEntity.FileName));

        await _mediator.Send(new DownloadFileCommand
        {
            Visibility = FileVisibility.Private,
            ObjectKey = fileEntity.ObjectKey,
            StoreFilePath = tempFile,
        });

        // 解析 openapi 文件，读取每个接口
        using var fileStream = new FileStream(tempFile, FileMode.Open);
        var reader = new OpenApiStreamReader();
        var apiReaderResult = await reader.ReadAsync(fileStream);

        List<TeamPluginEntity> pluginEntities = new List<TeamPluginEntity>();

        foreach (var pathEntry in apiReaderResult.OpenApiDocument.Paths)
        {
            // 接口名称
            var operationId = pathEntry.Value.Operations.First().Value.OperationId;
            var summary = pathEntry.Value.Operations.First().Value.Summary;

            pluginEntities.Add(new TeamPluginEntity
            {
                Name = operationId,
                Summary = summary,
                TeamId = request.TeamId,
                Path = pathEntry.Key
            });
        }

        using TransactionScope transactionScope = new TransactionScope(
            scopeOption: TransactionScopeOption.Required,
            asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled,
            transactionOptions: new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead });

        // 自动生成新的分组
        var pluginGroup = new TeamPluginGroupEntity
        {
            Server = apiReaderResult.OpenApiDocument.Servers.FirstOrDefault()?.Url ?? string.Empty,
            Name = request.Name,
            Type = (int)PluginType.OpenApi,
            TeamId = request.TeamId,
            Description = TruncateString(apiReaderResult.OpenApiDocument.Info.Description, 255),
            OpenapiFileId = fileEntity.Id,
            OpenapiFileName = fileEntity.FileName,
            Header = "{}",
            Query = "{}"
        };

        await _databaseContext.TeamPluginGroups.AddAsync(pluginGroup, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        foreach (var item in pluginEntities)
        {
            item.GroupId = pluginGroup.Id;
        }

        await _databaseContext.TeamPlugins.AddRangeAsync(pluginEntities, cancellationToken);
        await _databaseContext.SaveChangesAsync();

        transactionScope.Complete();

        return new IdResponse { Id = pluginGroup.Id };
    }

    public static string TruncateString(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

}