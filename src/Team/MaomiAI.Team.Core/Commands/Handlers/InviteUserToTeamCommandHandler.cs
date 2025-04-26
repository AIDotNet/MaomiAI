// <copyright file="InviteUserToTeamCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.AI.Exceptions;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Commands.Handlers;

/// <summary>
/// 处理邀请用户加入团队命令.
/// </summary>
public class InviteUserToTeamCommandHandler : IRequestHandler<InviteUserToTeamCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<InviteUserToTeamCommandHandler> _logger;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="InviteUserToTeamCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    /// <param name="userContext">用户上下文.</param>
    public InviteUserToTeamCommandHandler(
        DatabaseContext dbContext,
        ILogger<InviteUserToTeamCommandHandler> logger,
        UserContext userContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userContext = userContext;
    }

    /// <summary>
    /// 处理邀请用户加入团队命令.
    /// </summary>
    /// <param name="request">邀请用户加入团队命令.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>新创建的团队成员ID.</returns>
    /// <exception cref="InvalidOperationException">当团队不存在、用户不存在或用户已经是团队成员时抛出.</exception>
    public async Task<EmptyCommandResponse> Handle(InviteUserToTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _dbContext.Teams
            .FirstOrDefaultAsync(t => t.Id == request.TeamId, cancellationToken);

        if (team == null)
        {
            throw new BusinessException("团队不存在");
        }

        if (team.OwnerId == request.UserId)
        {
            throw new BusinessException("用户已经是团队成员");
        }

        var existUser = await _dbContext.Users
            .AnyAsync(u => u.Id == request.UserId, cancellationToken);

        if (existUser == false)
        {
            throw new BusinessException("用户不存在");
        }

        var existMember = await _dbContext.TeamMembers.AnyAsync(
            tm => tm.TeamId == request.TeamId && tm.UserId == request.UserId,
            cancellationToken);

        if (existMember)
        {
            throw new BusinessException("用户已经是团队成员");
        }

        await _dbContext.TeamMembers.AddAsync(new TeamMemberEntity
        {
            IsAdmin = false,
            TeamId = request.TeamId,
            UserId = request.UserId,
        });

        await _dbContext.SaveChangesAsync();

        return EmptyCommandResponse.Default;
    }
}