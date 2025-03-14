// <copyright file="RemoveTeamMemberCommandHandler.cs" company="MaomiAI">
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
/// 处理移除团队成员命令.
/// </summary>
public class RemoveTeamMemberCommandHandler : IRequestHandler<RemoveTeamMemberCommand>
{
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<RemoveTeamMemberCommandHandler> _logger;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveTeamMemberCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    /// <param name="userContext">用户上下文.</param>
    public RemoveTeamMemberCommandHandler(
        MaomiaiContext dbContext,
        ILogger<RemoveTeamMemberCommandHandler> logger,
        UserContext userContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userContext = userContext;
    }

    /// <summary>
    /// 处理移除团队成员命令.
    /// </summary>
    /// <param name="request">移除团队成员命令.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>任务.</returns>
    /// <exception cref="InvalidOperationException">当团队成员不存在时抛出.</exception>
    public async Task Handle(RemoveTeamMemberCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 获取当前用户ID
            var currentUserId = _userContext.UserId;

            // 查找团队成员
            var teamMember = await _dbContext.TeamMembers
                .FirstOrDefaultAsync(m => m.Id == request.MemberId && !m.IsDeleted, cancellationToken);

            if (teamMember == null)
            {
                _logger.LogWarning("尝试移除不存在的团队成员: {MemberId}", request.MemberId);
                throw new InvalidOperationException($"ID为{request.MemberId}的团队成员不存在");
            }

            // 验证操作人是否有权限（必须是团队管理员或所有者，或者是自己退出）
            var operatorMember = await _dbContext.TeamMembers
                .FirstOrDefaultAsync(m => m.TeamId == teamMember.TeamId && m.UserId == currentUserId && !m.IsDeleted, cancellationToken);

            if (operatorMember == null ||
                (!operatorMember.IsAdmin && !operatorMember.IsRoot && operatorMember.Id != request.MemberId))
            {
                _logger.LogWarning("用户 {OperatorId} 没有权限移除团队成员 {MemberId}", currentUserId, request.MemberId);
                throw new InvalidOperationException("您没有权限移除该团队成员");
            }

            // 不能移除团队所有者
            if (teamMember.IsRoot && operatorMember.Id != request.MemberId)
            {
                _logger.LogWarning("尝试移除团队所有者: {MemberId}", request.MemberId);
                throw new InvalidOperationException("不能移除团队所有者");
            }

            // 标记为删除
            teamMember.IsDeleted = true;
            teamMember.UpdateTime = DateTimeOffset.UtcNow;
            teamMember.UpdateUserId = currentUserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("成功移除团队成员: {MemberId}, 操作者: {OperatorId}", request.MemberId, currentUserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "移除团队成员失败: {MemberId}, {Message}", request.MemberId, ex.Message);
            throw;
        }
    }
}