// <copyright file="InviteUserToTeamCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Team.Shared.Commands.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Handlers;

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
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    /// <param name="userContext"></param>
    public InviteUserToTeamCommandHandler(
        DatabaseContext dbContext,
        ILogger<InviteUserToTeamCommandHandler> logger,
        UserContext userContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userContext = userContext;
    }

    /// <inheritdoc/>
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

        if (team.OwnerId == userId)
        {
            throw new BusinessException("用户已经是团队成员") { StatusCode = 409 };
        }

        var existMember = await _dbContext.TeamMembers.AnyAsync(
            tm => tm.TeamId == request.TeamId && tm.UserId == userId,
            cancellationToken);

        if (existMember)
        {
            throw new BusinessException("用户已经是团队成员") { StatusCode = 409 };
        }

        await _dbContext.TeamMembers.AddAsync(new TeamMemberEntity
        {
            IsAdmin = false,
            TeamId = request.TeamId,
            UserId = userId,
        });

        await _dbContext.SaveChangesAsync();

        return EmptyCommandResponse.Default;
    }
}