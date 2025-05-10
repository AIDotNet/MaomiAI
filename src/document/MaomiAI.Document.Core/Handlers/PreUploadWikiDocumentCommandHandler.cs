// <copyright file="PreUploadWikiDocumentCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Document.Shared.Commands;
using MaomiAI.Document.Shared.Commands.Responses;
using MaomiAI.Store.Commands;
using MaomiAI.Store.Enums;
using MaomiAI.Team.Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Handlers;

public class PreUploadWikiDocumentCommandHandler : IRequestHandler<PreUploadWikiDocumentCommand, PreloadWikiDocumentResponse>
{
    private readonly IMediator _mediator;
    private readonly DatabaseContext _databaseContext;

    public PreUploadWikiDocumentCommandHandler(IMediator mediator, DatabaseContext databaseContext)
    {
        _mediator = mediator;
        _databaseContext = databaseContext;
    }

    public async Task<PreloadWikiDocumentResponse> Handle(PreUploadWikiDocumentCommand request, CancellationToken cancellationToken)
    {
        if (!FileStoreHelper.DocumentFormats.Contains(Path.GetExtension(request.FileName).ToLower()))
        {
            throw new BusinessException("文件格式不支持") { StatusCode = 400 };
        }

        // 同一个知识库下不能有同名文件.
        var existFile = await _databaseContext.TeamWikiDocuments
            .Join(_databaseContext.Files.Where(x => x.FileName == request.FileName), a => a.FileId, b => b.Id, (a, b) => new { })
            .AnyAsync();

        if (existFile)
        {
            throw new BusinessException("已存在同名文件") { StatusCode = 409 };
        }

        var objectKey = FileStoreHelper.GetObjectKey(request.MD5, request.FileName, prefix: $"wiki/{request.WikiId}");

        var result = await _mediator.Send(new PreuploadFileCommand
        {
            MD5 = request.MD5,
            FileName = request.FileName,
            ContentType = request.ContentType,
            FileSize = request.FileSize,
            Visibility = FileVisibility.Private,
            ObjectKey = objectKey,
            Expiration = TimeSpan.FromMinutes(2),
        });

        var zipFile = await _databaseContext.TeamWikiDocuments.AddAsync(new Database.Entities.TeamWikiDocumentEntity
        {
            FileId = result.FileId,
            WikiId = request.WikiId,
        });

        if (result.IsExist)
        {
            throw new BusinessException("文件已存在") { StatusCode = 409 };
        }

        return new PreloadWikiDocumentResponse
        {
            Visibility = FileVisibility.Private,
            FileId = result.FileId,
            IsExist = result.IsExist,
            UploadUrl = result.UploadUrl,
            Expiration = result.Expiration,
        };
    }
}
