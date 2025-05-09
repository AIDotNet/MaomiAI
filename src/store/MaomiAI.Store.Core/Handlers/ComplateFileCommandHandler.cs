﻿// <copyright file="ComplateFileCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Store.Enums;
using MaomiAI.Store.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 完成文件上传.
/// </summary>
public class ComplateFileCommandHandler : IRequestHandler<ComplateFileUploadCommand, ComplateFileCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComplateFileCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="userContext"></param>
    public ComplateFileCommandHandler(DatabaseContext dbContext, IServiceProvider serviceProvider, UserContext userContext)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public async Task<ComplateFileCommandResponse> Handle(ComplateFileUploadCommand request, CancellationToken cancellationToken)
    {
        var file = await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == request.FileId, cancellationToken);

        if (file == null)
        {
            throw new BusinessException("文件不存在") { StatusCode = 404 };
        }

        // 文件早已上传完毕，忽略用户请求
        if (file.IsUpload)
        {
            return new ComplateFileCommandResponse();
        }

        // 检查该文件是否当前用户上传的，否则无法完成上传
        if (file.UpdateUserId != _userContext.UserId)
        {
            throw new BusinessException("文件不属于当前用户上传") { StatusCode = 403 };
        }

        // 无论成功失败，都先检查对象存储文件是否存在
        var fileStore = _serviceProvider.GetRequiredKeyedService<IFileStore>(file.IsPublic ? FileVisibility.Public : FileVisibility.Private);
        var fileSize = await fileStore.GetFileSizeAsync(file.ObjectKey);

        if (request.IsSuccess)
        {
            if (fileSize != file.FileSize)
            {
                _dbContext.Files.Remove(file);
                await _dbContext.SaveChangesAsync();

                throw new BusinessException("上传的文件已损坏") { StatusCode = 400 };
            }

            file.IsUpload = true;
            _dbContext.Update(file);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _dbContext.Files.Remove(file);
            await _dbContext.SaveChangesAsync();
        }

        return new ComplateFileCommandResponse();
    }
}
