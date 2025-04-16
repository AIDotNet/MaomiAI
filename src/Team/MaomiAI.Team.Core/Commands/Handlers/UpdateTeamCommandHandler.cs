// <copyright file="UpdateTeamCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.AI.Exceptions;
using MaomiAI.Database;
using MaomiAI.Infra.Models;
using MaomiAI.Store.Queries;
using MaomiAI.Team.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Commands.Handlers;

/// <summary>
/// 处理更新团队命令.
/// </summary>
public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand>
{
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<UpdateTeamCommandHandler> _logger;
    private readonly UserContext _userContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTeamCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    /// <param name="userContext"></param>
    /// <param name="mediator"></param>
    public UpdateTeamCommandHandler(
        MaomiaiContext dbContext,
        ILogger<UpdateTeamCommandHandler> logger,
        UserContext userContext,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userContext = userContext;
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _dbContext.Teams.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (team == null)
        {
            throw new BusinessException("团队不存在");
        }

        if (team.OwnerId != _userContext.UserId)
        {
            throw new BusinessException("没有权限修改该团队");
        }

        var anySameName = await _dbContext.Teams.Where(x => x.Id != request.Id && x.Name == request.Name).AnyAsync();
        if (anySameName)
        {
            throw new BusinessException("已存在相同名称的团队");
        }

        // 如果修改了图像文件，则检查文件是否存在
        if (request.AvatarFileId != Guid.Empty && request.AvatarFileId != team.AvatarFileId)
        {
            var existFile = await _mediator.Send(new CheckFileExistCommand
            {
                Visibility = Store.Enums.FileVisibility.Public,
                FileId = request.AvatarFileId,
            });

            if (!existFile.Exist)
            {
                throw new BusinessException("头像文件不存在");
            }
        }

        team.Name = request.Name;
        team.Description = request.Description;
        team.AvatarFileId = request.AvatarFileId;
        team.IsPublic = request.IsPublic;
        team.IsDisable = request.IsDisable;
        team.Markdown = request.Markdown ?? string.Empty;

        _dbContext.Update(team);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}