// <copyright file="SetTeamMemberPermissionCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.AI.Exceptions;
using MaomiAI.Database;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Commands.Handlers;

/// <summary>
/// 设置团队成员权限.
/// </summary>
public class SetTeamMemberPermissionCommandHandler : IRequestHandler<SetTeamMemberPermissionCommand, EmptyDto>
{
    private readonly MaomiaiContext _dbContext;
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
        MaomiaiContext dbContext,
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
    public async Task<EmptyDto> Handle(SetTeamMemberPermissionCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _userContext.UserId;

        var team = await _dbContext.Teams
            .FirstOrDefaultAsync(t => t.Id == request.TeamId && !t.IsDeleted, cancellationToken);

        if (team == null)
        {
            throw new BusinessException("团队不存在");
        }

        var teamMember = await _dbContext.TeamMembers
            .FirstOrDefaultAsync(x => x.TeamId == request.TeamId && x.UserId == request.UserId, cancellationToken);

        if (teamMember == null)
        {
            throw new BusinessException("团队成员不存在");
        }

        if (team.OwnerId == request.UserId)
        {
            throw new BusinessException("不可以设置团队所有者的权限");
        }

        var adminIds = await _mediator.Send(new QueryTeamAdminIdsListReuqest
        {
            TeamId = team.Id,
        });

        // 当前用户不是管理员禁止修改团队设置
        if (!adminIds.AdminIds.Contains(currentUserId))
        {
            throw new BusinessException("无团队管理员权限");
        }

        // 团队所有者可以设置管理员的权限
        if (adminIds.AdminIds.Contains(request.UserId) && currentUserId != team.OwnerId)
        {
            throw new BusinessException("不能修改团队管理员的权限");
        }

        if (request.IsAdmin != null)
        {
            if (currentUserId != team.OwnerId)
            {
                throw new BusinessException("无团队管理员权限");
            }
            else
            {
                teamMember.IsAdmin = request.IsAdmin.GetValueOrDefault();
            }
        }

        teamMember.IsEnable = request.IsEnable;

        _dbContext.Update(teamMember);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new EmptyDto();
    }
}