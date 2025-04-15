// <copyright file="TransferTeamOwnershipCommandHandler.cs" company="MaomiAI">
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
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Commands.Handlers
{
    /// <summary>
    /// 处理转移团队所有权命令.
    /// </summary>
    public class TransferTeamOwnershipCommandHandler : IRequestHandler<TransferTeamOwnershipCommand>
    {
        private readonly MaomiaiContext _dbContext;
        private readonly UserContext _userContext;
        private readonly ILogger<TransferTeamOwnershipCommandHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferTeamOwnershipCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        /// <param name="logger">日志记录器.</param>
        /// <param name="userContext">当前用户服务.</param>
        public TransferTeamOwnershipCommandHandler(
            MaomiaiContext dbContext,
            ILogger<TransferTeamOwnershipCommandHandler> logger,
            UserContext userContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userContext = userContext;
        }

        /// <summary>
        /// 处理转移团队所有权命令.
        /// </summary>
        /// <param name="request">转移团队所有权命令.</param>
        /// <param name="cancellationToken">取消令牌.</param>
        /// <returns>任务.</returns>
        /// <exception cref="InvalidOperationException">当团队不存在、用户不存在、用户不是团队成员或操作人不是团队所有者时抛出.</exception>
        public async Task Handle(TransferTeamOwnershipCommand request, CancellationToken cancellationToken)
        {
            // 获取当前用户ID
            Guid currentUserId = _userContext.UserId;

            // 验证团队是否存在
            TeamEntity? team = await _dbContext.Teams
                                   .FirstOrDefaultAsync(t => t.Id == request.TeamId && !t.IsDeleted,
                                       cancellationToken)
                               ?? throw new InvalidOperationException($"ID为{request.TeamId}的团队不存在");

            // 验证当前操作人是否是团队所有者
            TeamMemberEntity? currentOwner = await _dbContext.TeamMembers
                                                 .FirstOrDefaultAsync(
                                                     m => m.TeamId == request.TeamId && m.UserId == currentUserId &&
                                                          m.IsRoot, cancellationToken)
                                             ?? throw new InvalidOperationException("您不是该团队的所有者，无法转移所有权");

            // 验证新所有者是否是团队成员
            TeamMemberEntity? newOwner = await _dbContext.TeamMembers
                                             .FirstOrDefaultAsync(
                                                 m => m.TeamId == request.TeamId && m.UserId == request.NewOwnerId,
                                                 cancellationToken)
                                         ?? throw new InvalidOperationException("新所有者不是该团队的成员");

            // 开始事务
            using IDbContextTransaction? transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            // 更新当前所有者
            currentOwner.IsRoot = false;
            currentOwner.UpdateTime = DateTimeOffset.UtcNow;
            currentOwner.UpdateUserId = currentUserId;

            // 更新新所有者
            newOwner.IsRoot = true;
            newOwner.IsAdmin = true; // 所有者必须是管理员
            newOwner.UpdateTime = DateTimeOffset.UtcNow;
            newOwner.UpdateUserId = currentUserId;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
    }
}