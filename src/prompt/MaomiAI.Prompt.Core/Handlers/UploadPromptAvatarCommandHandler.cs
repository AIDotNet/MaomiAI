// <copyright file="UploadTeamAvatarCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands.Root;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Team.Core.Handlers;

/// <summary>
/// 修改团队头像.
/// </summary>
public class UploadPromptAvatarCommandHandler : IRequestHandler<UploadPromptAvatarCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UploadPromptAvatarCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="userContext"></param>
    public UploadPromptAvatarCommandHandler(DatabaseContext databaseContext, UserContext userContext)
    {
        _dbContext = databaseContext;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(UploadPromptAvatarCommand request, CancellationToken cancellationToken)
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

        var prompt = await _dbContext.Prompts.FirstOrDefaultAsync(x => x.Id == request.PromptId, cancellationToken);
        if (prompt == null)
        {
            throw new BusinessException("提示词不存在") { StatusCode = 400 };
        }

        prompt.AvatarPath = file.ObjectKey;

        _dbContext.Update(prompt);
        await _dbContext.SaveChangesAsync();

        return EmptyCommandResponse.Default;
    }
}
