﻿// <copyright file="UploadTeamAvatarCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Team.Shared.Commands.Root;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Team.Core.Handlers;

/// <summary>
/// 修改团队头像.
/// </summary>
public class UploadTeamAvatarCommandHandler : IRequestHandler<UploadTeamAvatarCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadTeamAvatarCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="userContext"></param>
    public UploadTeamAvatarCommandHandler(DatabaseContext databaseContext, UserContext userContext)
    {
        _dbContext = databaseContext;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(UploadTeamAvatarCommand request, CancellationToken cancellationToken)
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

        var team = await _dbContext.Teams.FirstOrDefaultAsync(x => x.Id == request.TeamId, cancellationToken);
        if (team == null)
        {
            throw new BusinessException("团队不存在") { StatusCode = 400 };
        }

        if (team.OwnerId != _userContext.UserId)
        {
            throw new BusinessException("没有权限修改团队头像") { StatusCode = 403 };
        }

        team.AvatarPath = file.ObjectKey;
        team.AvatarId = file.Id;

        _dbContext.Update(team);
        await _dbContext.SaveChangesAsync();

        return EmptyCommandResponse.Default;
    }
}
