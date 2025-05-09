// <copyright file="SetTeamMemberPermissionCommandHandler.cs" company="MaomiAI">
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
/// 设置团队成员权限，设置是否为管理员.
/// </summary>
public class SetTeamMemberPermissionCommandHandler : IRequestHandler<SetTeamAdminCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<SetTeamMemberPermissionCommandHandler> _logger;
    private readonly UserContext _userContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetTeamMemberPermissionCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    /// <param name="userContext"></param>
    /// <param name="mediator"></param>
    public SetTeamMemberPermissionCommandHandler(
        DatabaseContext dbContext,
        ILogger<SetTeamMemberPermissionCommandHandler> logger,
        UserContext userContext,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userContext = userContext;
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(SetTeamAdminCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _userContext.UserId;

        var team = await _dbContext.Teams
            .FirstOrDefaultAsync(t => t.Id == request.TeamId, cancellationToken);

        if (team == null)
        {
            throw new BusinessException("团队不存在") { StatusCode = 404 };
        }

        if (team.OwnerId != _userContext.UserId)
        {
            throw new BusinessException("没有权限操作") { StatusCode = 403 };
        }

        if (team.OwnerId == request.UserId)
        {
            throw new BusinessException("不可以设置团队所有者的权限") { StatusCode = 403 };
        }

        var userQuery = _dbContext.Users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.UserName))
        {
            userQuery = userQuery.Where(u => u.UserName == request.UserName);
        }
        else if (request.UserId != null)
        {
            userQuery = userQuery.Where(u => u.Id == request.UserId);
        }
        else
        {
            throw new BusinessException("用户ID或用户名不能为空") { StatusCode = 400 };
        }

        var userId = await userQuery.Select(x => x.Id).FirstOrDefaultAsync();
        if (userId == default)
        {
            throw new BusinessException("用户不存在") { StatusCode = 404 };
        }

        var teamMember = await _dbContext.TeamMembers
            .FirstOrDefaultAsync(x => x.TeamId == request.TeamId && x.UserId == userId, cancellationToken);

        if (teamMember == null)
        {
            throw new BusinessException("团队成员不存在") { StatusCode = 400 };
        }

        teamMember.IsAdmin = request.IsAdmin;

        _dbContext.Update(teamMember);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return EmptyCommandResponse.Default;
    }
}