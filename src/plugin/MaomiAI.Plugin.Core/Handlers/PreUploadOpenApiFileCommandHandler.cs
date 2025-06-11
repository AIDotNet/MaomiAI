// <copyright file="PreUploadOpenApiFileCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Plugin.Shared.Commands;
using MaomiAI.Plugin.Shared.Commands.Responses;
using MaomiAI.Store.Commands;
using MaomiAI.Store.Enums;
using MaomiAI.Team.Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Plugin.Core.Commands;

/// <summary>
/// 预上传 openapi 文件，支持 json、yaml.
/// </summary>
public class PreUploadOpenApiFileCommandHandler : IRequestHandler<PreUploadOpenApiFileCommand, PreUploadOpenApiFileCommandResponse>
{
    private readonly IMediator _mediator;
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreUploadOpenApiFileCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="databaseContext"></param>
    public PreUploadOpenApiFileCommandHandler(IMediator mediator, DatabaseContext databaseContext)
    {
        _mediator = mediator;
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<PreUploadOpenApiFileCommandResponse> Handle(PreUploadOpenApiFileCommand request, CancellationToken cancellationToken)
    {
        if (!FileStoreHelper.OpenApiFormats.Contains(Path.GetExtension(request.FileName).ToLower()))
        {
            throw new BusinessException("文件格式不支持") { StatusCode = 400 };
        }

        // 检查文件.
        var objectKey = FileStoreHelper.GetObjectKey(md5: request.MD5, fileName: request.FileName, prefix: $"plugin/{request.TeamId}");

        var existFile = await _databaseContext.Files.Where(x => x.ObjectKey == objectKey).FirstOrDefaultAsync();

        if (existFile != null)
        {
            return new PreUploadOpenApiFileCommandResponse
            {
                FileId = existFile.Id,
                IsExist = true,
            };
        }

        var result = await _mediator.Send(new PreUploadFileCommand
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
            return new PreUploadOpenApiFileCommandResponse
            {
                FileId = result.FileId,
                IsExist = true,
            };
        }

        return new PreUploadOpenApiFileCommandResponse
        {
            Visibility = FileVisibility.Private,
            FileId = result.FileId,
            IsExist = result.IsExist,
            UploadUrl = result.UploadUrl,
            Expiration = result.Expiration
        };
    }
}