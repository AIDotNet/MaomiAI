// <copyright file="UpdateTeamMemberRoleCommandHandler.cs" company="MaomiAI">
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

namespace MaomiAI.Team.Core.Commands.Handlers
{
    /// <summary>
    /// 处理更新团队成员角色命令.
    /// </summary>
    public class UpdateTeamMemberRoleCommandHandler : IRequestHandler<UpdateTeamMemberRoleCommand>
    {
        private readonly MaomiaiContext _dbContext;
        private readonly ILogger<UpdateTeamMemberRoleCommandHandler> _logger;
        private readonly UserContext _userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTeamMemberRoleCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        /// <param name="logger">日志记录器.</param>
        /// <param name="userContext">用户上下文.</param>
        public UpdateTeamMemberRoleCommandHandler(
            MaomiaiContext dbContext,
            ILogger<UpdateTeamMemberRoleCommandHandler> logger,
            UserContext userContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userContext = userContext;
        }

        /// <summary>
        /// 处理更新团队成员角色命令.
        /// </summary>
        /// <param name="request">更新团队成员角色命令.</param>
        /// <param name="cancellationToken">取消令牌.</param>
        /// <returns>任务.</returns>
        /// <exception cref="InvalidOperationException">当团队不存在、成员不存在或操作人没有权限时抛出.</exception>
        public async Task Handle(UpdateTeamMemberRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 获取当前用户ID
                Guid currentUserId = _userContext.UserId;

                // 验证团队是否存在
                TeamEntity? team = await _dbContext.Teams
                    .FirstOrDefaultAsync(t => t.Uuid == request.TeamId && !t.IsDeleted, cancellationToken);

                if (team == null)
                {
                    _logger.LogWarning("尝试更新不存在的团队的成员角色: {TeamId}", request.TeamId);
                    throw new InvalidOperationException($"ID为{request.TeamId}的团队不存在");
                }

                // 验证要更新的成员是否存在
                TeamMemberEntity? memberToUpdate = await _dbContext.TeamMembers
                    .FirstOrDefaultAsync(m => m.TeamId == request.TeamId &&
                                              m.UserId == request.MemberUserId &&
                                              !m.IsDeleted,
                        cancellationToken);

                if (memberToUpdate == null)
                {
                    _logger.LogWarning("尝试更新不存在的团队成员角色: 团队 {TeamId}, 用户 {MemberUserId}",
                        request.TeamId, request.MemberUserId);
                    throw new InvalidOperationException("该用户不是团队成员");
                }

                // 不能更改团队所有者的角色
                if (memberToUpdate.IsRoot)
                {
                    _logger.LogWarning("尝试更改团队所有者的角色: 团队 {TeamId}, 用户 {MemberUserId}",
                        request.TeamId, request.MemberUserId);
                    throw new InvalidOperationException("不能更改团队所有者的角色");
                }

                // 验证操作人是否有权限（必须是团队所有者）
                TeamMemberEntity? operatorMember = await _dbContext.TeamMembers
                    .FirstOrDefaultAsync(m => m.TeamId == request.TeamId &&
                                              m.UserId == currentUserId &&
                                              m.IsRoot &&
                                              !m.IsDeleted,
                        cancellationToken);

                if (operatorMember == null)
                {
                    _logger.LogWarning("用户 {OperatorId} 没有权限更新团队 {TeamId} 的成员角色",
                        currentUserId, request.TeamId);
                    throw new InvalidOperationException("只有团队所有者才能更改成员角色");
                }

                // 更新成员角色
                memberToUpdate.IsAdmin = request.IsAdmin;
                memberToUpdate.UpdateTime = DateTimeOffset.UtcNow;
                memberToUpdate.UpdateUserId = currentUserId;

                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("成功更新团队 {TeamId} 的成员 {MemberUserId} 的角色, 操作者: {OperatorId}, 新角色: {IsAdmin}",
                    request.TeamId, request.MemberUserId, currentUserId, request.IsAdmin ? "管理员" : "普通成员");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新团队 {TeamId} 的成员 {MemberUserId} 的角色失败: {Message}",
                    request.TeamId, request.MemberUserId, ex.Message);
                throw;
            }
        }
    }
}