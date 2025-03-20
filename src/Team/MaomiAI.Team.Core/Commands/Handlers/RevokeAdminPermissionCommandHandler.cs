// <copyright file="RevokeAdminPermissionCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System;
using System.Threading;
using System.Threading.Tasks;

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Commands.Handlers;

/// <summary>
/// 处理撤销团队成员管理员权限命令.
/// </summary>
public class RevokeAdminPermissionCommandHandler : IRequestHandler<RevokeAdminPermissionCommand, bool>
{
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<RevokeAdminPermissionCommandHandler> _logger;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="RevokeAdminPermissionCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    /// <param name="userContext">用户上下文.</param>
    public RevokeAdminPermissionCommandHandler(
        MaomiaiContext dbContext,
        ILogger<RevokeAdminPermissionCommandHandler> logger,
        UserContext userContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userContext = userContext;
    }

    /// <summary>
    /// 处理撤销团队成员管理员权限命令.
    /// </summary>
    /// <param name="request">撤销团队成员管理员权限命令.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>操作是否成功.</returns>
    /// <exception cref="InvalidOperationException">当团队不存在、用户不存在、用户不是团队成员或操作人没有权限时抛出.</exception>
    public async Task<bool> Handle(RevokeAdminPermissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 获取当前用户ID
            var currentUserId = _userContext.UserId;

            // 验证团队是否存在
            var team = await _dbContext.Teams
                .FirstOrDefaultAsync(t => t.Uuid == request.TeamId && !t.IsDeleted, cancellationToken);

            if (team == null)
            {
                _logger.LogWarning("尝试撤销管理员权限的团队不存在: {TeamId}", request.TeamId);
                throw new InvalidOperationException($"ID为{request.TeamId}的团队不存在");
            }

            // 验证操作人是否有权限（必须是团队所有者）
            var operatorMember = await _dbContext.TeamMembers
                .FirstOrDefaultAsync(m => m.TeamId == request.TeamId && m.UserId == currentUserId && !m.IsDeleted, cancellationToken);

            if (operatorMember == null || !operatorMember.IsRoot)
            {
                _logger.LogWarning("用户 {OperatorId} 没有权限撤销团队 {TeamId} 的管理员权限", currentUserId, request.TeamId);
                throw new InvalidOperationException("只有团队创建人才能撤销管理员权限");
            }

            // 验证目标用户是否是团队成员
            var targetMember = await _dbContext.TeamMembers
                .FirstOrDefaultAsync(m => m.TeamId == request.TeamId && m.UserId == request.UserId && !m.IsDeleted, cancellationToken);

            if (targetMember == null)
            {
                _logger.LogWarning("目标用户 {UserId} 不是团队 {TeamId} 的成员", request.UserId, request.TeamId);
                throw new InvalidOperationException("该用户不是团队成员");
            }

            // 检查目标用户是否是根用户（创建人）
            if (targetMember.IsRoot)
            {
                _logger.LogWarning("尝试撤销团队创建人 {UserId} 的管理员权限", request.UserId);
                throw new InvalidOperationException("不能撤销团队创建人的权限");
            }

            // 检查目标用户是否已经是管理员
            if (!targetMember.IsAdmin)
            {
                _logger.LogWarning("目标用户 {UserId} 不是团队 {TeamId} 的管理员", request.UserId, request.TeamId);
                throw new InvalidOperationException("该用户不是团队管理员");
            }

            // 更新用户为非管理员
            targetMember.IsAdmin = false;
            targetMember.UpdateTime = DateTimeOffset.UtcNow;
            targetMember.UpdateUserId = currentUserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("成功撤销用户 {UserId} 在团队 {TeamId} 的管理员权限, 操作者: {OperatorId}", request.UserId, request.TeamId, currentUserId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "撤销用户 {UserId} 在团队 {TeamId} 的管理员权限失败: {Message}", request.UserId, request.TeamId, ex.Message);
            throw;
        }
    }
}