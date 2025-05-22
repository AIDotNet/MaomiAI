// <copyright file="UploadLocalFilesCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Store.Commands;
using MaomiAI.Store.Enums;
using MaomiAI.Store.Queries;
using MaomiAI.Store.Services;
using MaomiAI.Team.Shared.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;

namespace MaomiAI.Store.Handlers;

public class UploadLocalFilesCommandHandler : IRequestHandler<UploadLocalFilesCommand, UploadLocalFilesCommandResponse>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DatabaseContext _databaseContext;

    public UploadLocalFilesCommandHandler(IServiceProvider serviceProvider, DatabaseContext databaseContext)
    {
        _serviceProvider = serviceProvider;
        _databaseContext = databaseContext;
    }

    public async Task<EmptyCommandResponse> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
    {
        var fileStore = _serviceProvider.GetRequiredKeyedService<IFileStore>(request.Visibility);

        await fileStore.DownloadAsync(request.ObjectKey, request.StoreFilePath);

        return EmptyCommandResponse.Default;
    }

    /// <inheritdoc/>
    public async Task<UploadLocalFilesCommandResponse> Handle(UploadLocalFilesCommand request, CancellationToken cancellationToken)
    {
        List<FileEntity> fileEntities = new List<FileEntity>();
        var uploadedFiles = new List<FileUploadResult>();

        var fileStore = _serviceProvider.GetRequiredKeyedService<IFileStore>(request.Visibility);

        foreach (var item in request.Files)
        {
            using var fileStream = new FileStream(item.FilePath, FileMode.Open);
            await fileStore.UploadFileAsync(fileStream, item.ObjectKey);

            fileEntities.Add(new FileEntity
            {
                FileName = item.FileName,
                ObjectKey = item.ObjectKey,
                FileMd5 = item.MD5,
                FileSize = fileStream.Length,
                IsPublic = request.Visibility == FileVisibility.Public,
                IsUpload = true,
                ContentType = item.ContentType,
            });
        }

        await _databaseContext.Files.AddRangeAsync(fileEntities, cancellationToken: cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        foreach (var item in fileEntities)
        {
            uploadedFiles.Add(new FileUploadResult
            {
                FileId = item.Id,
                FileName = item.FileName,
                ObjectKey = item.ObjectKey,
            });
        }

        return new UploadLocalFilesCommandResponse
        {
            Files = uploadedFiles
        };
    }
}
