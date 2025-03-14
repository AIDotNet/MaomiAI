// <copyright file="InviteUserToTeamCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Commands.Handlers;

/// <summary>
/// 处理邀请用户加入团队命令.
/// </summary>
public class InviteUserToTeamCommandHandler : IRequestHandler<InviteUserToTeamCommand, int>
{
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<InviteUserToTeamCommandHandler> _logger;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="InviteUserToTeamCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    /// <param name="userContext">用户上下文.</param>
    public InviteUserToTeamCommandHandler(
        MaomiaiContext dbContext,
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
    public async Task<int> Handle(InviteUserToTeamCommand request, CancellationToken cancellationToken)
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
                _logger.LogWarning("尝试邀请用户加入不存在的团队: {TeamId}", request.TeamId);
                throw new InvalidOperationException($"ID为{request.TeamId}的团队不存在");
            }

            // 验证用户是否存在
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("尝试邀请不存在的用户: {UserId}", request.UserId);
                throw new InvalidOperationException($"ID为{request.UserId}的用户不存在");
            }

            // 验证操作人是否有权限（必须是团队管理员或所有者）
            var operatorMember = await _dbContext.TeamMembers
                .FirstOrDefaultAsync(m => m.TeamId == request.TeamId && m.UserId == currentUserId && !m.IsDeleted, cancellationToken);

            if (operatorMember == null || (!operatorMember.IsAdmin && !operatorMember.IsRoot))
            {
                _logger.LogWarning("用户 {OperatorId} 没有权限邀请成员加入团队 {TeamId}", currentUserId, request.TeamId);
                throw new InvalidOperationException("您没有权限邀请成员加入该团队");
            }

            // 检查用户是否已经是团队成员
            var existingMember = await _dbContext.TeamMembers
                .FirstOrDefaultAsync(m => m.TeamId == request.TeamId && m.UserId == request.UserId && !m.IsDeleted, cancellationToken);

            if (existingMember != null)
            {
                _logger.LogWarning("用户 {UserId} 已经是团队 {TeamId} 的成员", request.UserId, request.TeamId);
                throw new InvalidOperationException("该用户已经是团队成员");
            }

            // 创建新的团队成员
            var teamMember = new TeamMemberEntity
            {
                TeamId = request.TeamId,
                UserId = request.UserId,
                IsRoot = false, // 只有创建团队的人才是Root
                IsAdmin = request.IsAdmin,
                IsDeleted = false,
                CreateTime = DateTimeOffset.UtcNow,
                UpdateTime = DateTimeOffset.UtcNow,
                CreateUserId = currentUserId,
                UpdateUserId = currentUserId
            };

            await _dbContext.TeamMembers.AddAsync(teamMember, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("成功邀请用户 {UserId} 加入团队 {TeamId}, 操作者: {OperatorId}, 是否为管理员: {IsAdmin}", request.UserId, request.TeamId, currentUserId, request.IsAdmin);

            return teamMember.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "邀请用户 {UserId} 加入团队 {TeamId} 失败: {Message}", request.UserId, request.TeamId, ex.Message);
            throw;
        }
    }
}