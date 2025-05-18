// <copyright file="UploadWikiAvatarCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Document.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Handlers;

/// <summary>
/// 上传知识库头像.
/// </summary>
public class UploadWikiAvatarCommandHandler : IRequestHandler<UploadWikiAvatarCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadWikiAvatarCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="mediator"></param>
    public UploadWikiAvatarCommandHandler(DatabaseContext databaseContext, IMediator mediator)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(UploadWikiAvatarCommand request, CancellationToken cancellationToken)
    {
        var file = await _databaseContext.Files.FirstOrDefaultAsync(x => x.Id == request.FileId);
        if (file == null)
        {
            throw new BusinessException("头像文件不存在") { StatusCode = 400 };
        }

        if (!file.IsUpload)
        {
            throw new BusinessException("头像文件尚未上传完毕") { StatusCode = 400 };
        }

        var wiki = await _databaseContext.TeamWikis.FirstOrDefaultAsync(x => x.Id == request.WikiId, cancellationToken);
        if (wiki == null)
        {
            throw new BusinessException("知识库不存在") { StatusCode = 400 };
        }

        wiki.AvatarPath = file.ObjectKey;
        wiki.AvatarId = file.Id;

        _databaseContext.Update(wiki);
        await _databaseContext.SaveChangesAsync();

        return EmptyCommandResponse.Default;
    }
}
