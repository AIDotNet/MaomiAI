// <copyright file="RemoveTeamMemberCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Team.Shared.Commands.Admin;
using MaomiAI.Team.Shared.Queries.Admin;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Handlers;

/// <summary>
/// 处理移除团队成员命令.
/// </summary>
public class RemoveTeamMemberCommandHandler : IRequestHandler<RemoveTeamMemberCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<RemoveTeamMemberCommandHandler> _logger;
    private readonly UserContext _userContext;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveTeamMemberCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    /// <param name="userContext">用户上下文.</param>
    /// <param name="mediator"></param>
    public RemoveTeamMemberCommandHandler(
        DatabaseContext dbContext,
        ILogger<RemoveTeamMemberCommandHandler> logger,
        UserContext userContext,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userContext = userContext;
        _mediator = mediator;
    }

    /// <summary>
    /// 处理移除团队成员命令.
    /// </summary>
    /// <param name="request">移除团队成员命令.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>任务.</returns>
    /// <exception cref="InvalidOperationException">当团队成员不存在时抛出.</exception>
    public async Task<EmptyCommandResponse> Handle(RemoveTeamMemberCommand request, CancellationToken cancellationToken)
    {
        var adminIds = await _mediator.Send(new QueryTeamAdminIdsListCommand { TeamId = request.TeamId }, cancellationToken);

        if (request.UserId == adminIds.OwnId)
        {
            throw new BusinessException("不可以移除团队所有者");
        }

        var teamMember = await _dbContext.TeamMembers
            .FirstOrDefaultAsync(x => x.TeamId == request.TeamId && x.UserId == request.UserId, cancellationToken);
        if (teamMember == null)
        {
            return EmptyCommandResponse.Default;
        }

        // 如果移除管理员
        if (adminIds.AdminIds.Contains(request.UserId))
        {
            if (adminIds.OwnId != _userContext.UserId)
            {
                throw new BusinessException("没有权限移除团队成员");
            }
            else
            {
                _dbContext.TeamMembers.Remove(teamMember);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return EmptyCommandResponse.Default;
        }

        // 需要管理员权限才能操作
        if (!adminIds.AdminIds.Contains(_userContext.UserId))
        {
            throw new BusinessException("没有权限移除团队成员");
        }

        _dbContext.TeamMembers.Remove(teamMember);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return EmptyCommandResponse.Default;
    }
}