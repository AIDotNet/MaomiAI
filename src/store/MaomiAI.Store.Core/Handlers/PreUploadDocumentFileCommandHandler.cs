// <copyright file="PreUploadDocumentFileCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.AI.Exceptions;
using MaomiAI.Store.Commands.Response;
using MaomiAI.Store.InternalCommands;
using MaomiAI.Team.Shared.Helpers;
using MediatR;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 预上传文档文件.
/// </summary>
public class PreUploadDocumentFileCommandHandler : IRequestHandler<InternalPreUploadDocumentFileCommand, PreUploadFileCommandResponse>
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreUploadDocumentFileCommandHandler"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public PreUploadDocumentFileCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<PreUploadFileCommandResponse> Handle(InternalPreUploadDocumentFileCommand request, CancellationToken cancellationToken)
    {
        if (FileStoreHelper.DocumentFormats.Contains(request.FileName.Split('.').Last()))
        {
            throw new BusinessException("文件格式不正确");
        }

        // todo: 限制头像文件大小.

        var preu = new InternalPreuploadFileCommand
        {
            FileName = request.FileName,
            ContentType = request.ContentType,
            FileSize = request.FileSize,
            MD5 = request.MD5,
            Expiration = TimeSpan.FromMinutes(1),
            Visibility = Enums.FileVisibility.Public,
            Path = FileStoreHelper.GetObjectKey(request.MD5, request.FileName)
        };

        return await _mediator.Send(preu, cancellationToken);
    }
}