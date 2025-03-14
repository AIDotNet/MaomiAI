// <copyright file="ToggleTeamStatusCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Commands.Handlers;

/// <summary>
/// 处理切换团队状态命令.
/// </summary>
public class ToggleTeamStatusCommandHandler : IRequestHandler<ToggleTeamStatusCommand>
{
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<ToggleTeamStatusCommandHandler> _logger;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleTeamStatusCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    /// <param name="userContext">用户上下文.</param>
    public ToggleTeamStatusCommandHandler(
        MaomiaiContext dbContext,
        ILogger<ToggleTeamStatusCommandHandler> logger,
        UserContext userContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userContext = userContext;
    }

    /// <summary>
    /// 处理切换团队状态命令.
    /// </summary>
    /// <param name="request">切换团队状态命令.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>任务.</returns>
    public async Task Handle(ToggleTeamStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 获取当前用户ID
            var currentUserId = _userContext.UserId;

            // 查找需要更新的团队
            var teamsToUpdate = await _dbContext.Teams
                .Where(t => request.TeamIds.Contains(t.Uuid) && !t.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!teamsToUpdate.Any())
            {
                _logger.LogWarning("没有找到需要更新状态的团队");
                return;
            }

            // 验证操作人是否有权限（必须是团队管理员或所有者）
            foreach (var team in teamsToUpdate)
            {
                var operatorMember = await _dbContext.TeamMembers
                    .FirstOrDefaultAsync(m => m.TeamId == team.Uuid && m.UserId == currentUserId && !m.IsDeleted, cancellationToken);

                if (operatorMember == null || (!operatorMember.IsAdmin && !operatorMember.IsRoot))
                {
                    _logger.LogWarning("用户 {OperatorId} 没有权限更新团队 {TeamId} 的状态", currentUserId, team.Uuid);
                    throw new InvalidOperationException($"您没有权限更新团队 {team.Name} 的状态");
                }

                // 更新团队状态
                team.ChangeStatus(request.Status, currentUserId);
            }

            var count = await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("成功更新 {Count} 个团队的状态为 {Status}, 操作者: {OperatorId}", count, request.Status, currentUserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新团队状态失败: {Message}", ex.Message);
            throw;
        }
    }
}