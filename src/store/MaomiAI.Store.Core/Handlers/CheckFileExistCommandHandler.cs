// <copyright file="CheckFileExistCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using LinqKit;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Store.Queries;
using MaomiAI.Store.Queries.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 检查文件是否存在.
/// </summary>
public class CheckFileExistCommandHandler : IRequestHandler<CheckFileExistCommand, CheckFileExistResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckFileExistCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="serviceProvider"></param>
    public CheckFileExistCommandHandler(DatabaseContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task<CheckFileExistResponse> Handle(CheckFileExistCommand request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Files.AsQueryable();
        bool hasVisibility = false;

        var predicate = PredicateBuilder.New<FileEntity>();

        if (request.FileId != null)
        {
            predicate = predicate.Or(x => x.Id == request.FileId);
        }

        if (!string.IsNullOrEmpty(request.MD5))
        {
            hasVisibility = true;
            predicate = predicate.Or(x => x.FileMd5 == request.MD5);
        }

        if (!string.IsNullOrEmpty(request.Key))
        {
            hasVisibility = true;
            predicate = predicate.Or(x => x.ObjectKey == request.Key);
        }

        query = query.Where(predicate);
        if (hasVisibility)
        {
            var isPublicFile = request.Visibility == Enums.FileVisibility.Public ? true : false;
            query = query.Where(x => x.IsPublic == isPublicFile);
        }

        var existFile = await query.AnyAsync(cancellationToken);
        return new CheckFileExistResponse
        {
            Exist = existFile
        };
    }
}