// <copyright file="UpdateTeamCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Team.Shared.Commands.Root;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Handlers;

/// <summary>
/// 处理更新团队命令.
/// </summary>
public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamInfoCommand>
{
    private readonly DatabaseContext _dbContext;
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
        DatabaseContext dbContext,
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
    public async Task Handle(UpdateTeamInfoCommand request, CancellationToken cancellationToken)
    {
        var team = await _dbContext.Teams.FirstOrDefaultAsync(t => t.Id == request.TeamId, cancellationToken);
        if (team == null)
        {
            throw new BusinessException("团队不存在") { StatusCode = 404 };
        }

        if (team.OwnerId != _userContext.UserId)
        {
            throw new BusinessException("没有权限修改该团队") { StatusCode = 403 };
        }

        var anySameName = await _dbContext.Teams.Where(x => x.Id != request.TeamId && x.Name == request.Name).AnyAsync();
        if (anySameName)
        {
            throw new BusinessException("已存在相同名称的团队") { StatusCode = 400 }; ;
        }

        team.Name = request.Name;
        team.Description = request.Description;
        team.IsPublic = request.IsPublic;
        team.IsDisable = request.IsDisable;
        team.Markdown = request.Markdown ?? string.Empty;

        _dbContext.Update(team);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}