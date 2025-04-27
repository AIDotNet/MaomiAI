// <copyright file="UploadtUserAvatarCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.AI.Exceptions;
using MaomiAI.Database;
using MaomiAI.Infra.Models;
using MaomiAI.User.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.User.Core.Handlers;

public class UploadtUserAvatarCommandHandler : IRequestHandler<UploadtUserAvatarCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly UserContext _userContext;
    private readonly IMediator _mediator;

    public UploadtUserAvatarCommandHandler(DatabaseContext databaseContext, UserContext userContext, IMediator mediator)
    {
        _dbContext = databaseContext;
        _userContext = userContext;
        _mediator = mediator;
    }

    public async Task<EmptyCommandResponse> Handle(UploadtUserAvatarCommand request, CancellationToken cancellationToken)
    {
        var file = await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == request.FileId);
        if (file == null)
        {
            throw new BusinessException("头像文件不存在") { StatusCode = 400 };
        }

        if (!file.IsUpload)
        {
            throw new BusinessException("头像文件尚未上传完毕") { StatusCode = 400 };
        }

        await _mediator.Send(new CheckUserStateCommand { UserId = _userContext.UserId }, cancellationToken);

        var user = (await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == _userContext.UserId, cancellationToken))!;
        user.AvatarPath = file.Path;
        user.AvatarId = file.Id;

        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync();

        return EmptyCommandResponse.Default;
    }
}