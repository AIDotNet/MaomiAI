// <copyright file="PreuploadFileCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Store.Enums;
using MaomiAI.Store.Services;
using MaomiAI.Team.Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 预上传文件.
/// </summary>
public class PreuploadFileCommandHandler : IRequestHandler<InternalPreuploadFileCommand, PreUploadFileCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreuploadFileCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="serviceProvider"></param>
    public PreuploadFileCommandHandler(DatabaseContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async Task<PreUploadFileCommandResponse> Handle(InternalPreuploadFileCommand request, CancellationToken cancellationToken)
    {
        var isPublic = request.Visibility == FileVisibility.Public ? true : false;

        // 如果文件的 md5 已存在并且文件大小相同，则直接返回文件的 oss 地址，无需重复上传
        // public 和 private 不可以是同一个桶
        var file = await _dbContext.Files
            .FirstOrDefaultAsync(x => x.FileMd5 == request.MD5 && x.FileSize == request.FileSize && x.IsPublic == isPublic, cancellationToken);

        // 文件已存在
        if (file != null && file.IsUpload)
        {
            return new PreUploadFileCommandResponse
            {
                IsExist = true,
                FileId = file.Id
            };
        }

        FileEntity fileEntity;
        if (file == null)
        {
            fileEntity = new FileEntity
            {
                FileName = request.FileName,
                FileMd5 = request.MD5,
                FileSize = request.FileSize,
                ContentType = request.ContentType,
                IsUpload = false,
                IsPublic = isPublic,
                Path = request.Path
            };

            await _dbContext.Files.AddAsync(fileEntity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            // 复用相同的 id
            fileEntity = file;
        }

        var fileStore = _serviceProvider.GetRequiredKeyedService<IFileStore>(request.Visibility);

        var uploadUrl = await fileStore.GeneratePreSignedUploadUrlAsync(new FileObject
        {
            ExpiryDuration = request.Expiration,
            ObjectKey = request.Path,
            ContentType = request.ContentType,
            MaxFileSize = FileStoreHelper.GetAllowedFileSizeLimit(request.FileSize)
        });

        return new PreUploadFileCommandResponse
        {
            IsExist = false,
            Expiration = DateTimeOffset.Now.Add(request.Expiration),
            FileId = fileEntity.Id,
            UploadUrl = new Uri(uploadUrl)
        };
    }
}