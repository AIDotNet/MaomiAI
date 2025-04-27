// <copyright file="CheckFileExistCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using MaomiAI.Database;
using MaomiAI.Infra;
using MaomiAI.Store.Enums;
using MaomiAI.Store.Queries;
using MaomiAI.Store.Queries.Response;
using MaomiAI.Store.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 获取文件路径.
/// </summary>
public class QueryPublicFilePathCommandHandler : IRequestHandler<QueryPublicFilePathCommand, QueryPublicFilePathCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryPublicFilePathCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="systemOptions"></param>
    public QueryPublicFilePathCommandHandler(DatabaseContext dbContext, IServiceProvider serviceProvider, SystemOptions systemOptions)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
        _systemOptions = systemOptions;
    }

    /// <inheritdoc/>
    public async Task<QueryPublicFilePathCommandResponse> Handle(QueryPublicFilePathCommand request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Files.Where(x => x.IsPublic == true);
        if (request.FileId != null)
        {
            query = query.Where(x => x.Id == request.FileId);
        }

        if (!string.IsNullOrEmpty(request.MD5))
        {
            query = query.Where(x => x.FileMd5 == request.MD5);
        }

        if (!string.IsNullOrEmpty(request.Key))
        {
            query = query.Where(x => x.Path == request.Key);
        }

        var existFile = await query.FirstOrDefaultAsync(cancellationToken);
        if (existFile == null)
        {
            return new QueryPublicFilePathCommandResponse
            {
                Exist = false
            };
        }

        var url = Path.Combine(_systemOptions.PublicStore.Endpoint, existFile.Path);

        return new QueryPublicFilePathCommandResponse
        {
            Exist = true,
            Path = existFile.Path,
            Url = url,
        };
    }
}