// <copyright file="PreUploadWikiDocumentCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Document.Shared.Commands.Documents;
using MaomiAI.Document.Shared.Commands.Responses;
using MaomiAI.Store.Commands;
using MaomiAI.Store.Enums;
using MaomiAI.Team.Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Handlers.Documents;

/// <summary>
/// 预上传知识库文件.
/// </summary>
public class PreUploadWikiDocumentCommandHandler : IRequestHandler<PreUploadWikiDocumentCommand, PreloadWikiDocumentResponse>
{
    private readonly IMediator _mediator;
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreUploadWikiDocumentCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="databaseContext"></param>
    public PreUploadWikiDocumentCommandHandler(IMediator mediator, DatabaseContext databaseContext)
    {
        _mediator = mediator;
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<PreloadWikiDocumentResponse> Handle(PreUploadWikiDocumentCommand request, CancellationToken cancellationToken)
    {
        if (!FileStoreHelper.DocumentFormats.Contains(Path.GetExtension(request.FileName).ToLower()))
        {
            throw new BusinessException("文件格式不支持") { StatusCode = 400 };
        }

        // 同一个知识库下不能有同名文件.
        var existFileCount = await _databaseContext.TeamWikiDocuments.Where(x => x.FileName == request.FileName).CountAsync();

        if (existFileCount > 0)
        {
            throw new BusinessException("同一个知识库下不能有同名文件") { StatusCode = 409 };
        }

        var objectKey = FileStoreHelper.GetObjectKey(md5: request.MD5, fileName: request.FileName, prefix: $"wiki/{request.WikiId}");

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

        if (result.IsExist)
        {
            throw new BusinessException("同一个知识库下不能有同名文件，请联系管理员") { StatusCode = 409 };
        }

        var documentFile = await _databaseContext.TeamWikiDocuments.AddAsync(new Database.Entities.TeamWikiDocumentEntity
        {
            FileId = result.FileId,
            WikiId = request.WikiId,
            TeamId = request.TeamId,
            FileName = request.FileName,
        });

        await _databaseContext.SaveChangesAsync();

        return new PreloadWikiDocumentResponse
        {
            Visibility = FileVisibility.Private,
            FileId = result.FileId,
            IsExist = result.IsExist,
            UploadUrl = result.UploadUrl,
            Expiration = result.Expiration
        };
    }
}
