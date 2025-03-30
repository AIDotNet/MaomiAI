// <copyright file="CheckTeamAdminQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Queries.Handlers
{
    /// <summary>
    /// 处理检查用户是否是团队管理员的查询.
    /// </summary>
    public class CheckTeamAdminQueryHandler : IRequestHandler<CheckTeamAdminQuery, bool>
    {
        private readonly MaomiaiContext _dbContext;
        private readonly ILogger<CheckTeamAdminQueryHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckTeamAdminQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        /// <param name="logger">日志记录器.</param>
        public CheckTeamAdminQueryHandler(MaomiaiContext dbContext, ILogger<CheckTeamAdminQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// 处理检查用户是否是团队管理员的查询.
        /// </summary>
        /// <param name="request">查询请求.</param>
        /// <param name="cancellationToken">取消令牌.</param>
        /// <returns>如果用户是团队管理员则返回true，否则返回false.</returns>
        public async Task<bool> Handle(CheckTeamAdminQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("检查用户是否是团队管理员: TeamId={TeamId}, UserId={UserId}", request.TeamId,
                    request.UserId);

                // 首先检查团队是否存在
                bool teamExists = await _dbContext.Teams
                    .AnyAsync(t => t.Id == request.TeamId && !t.IsDeleted, cancellationToken);

                if (!teamExists)
                {
                    _logger.LogWarning("团队不存在: TeamId={TeamId}", request.TeamId);
                    return false;
                }

                // 检查用户是否是团队管理员或所有者
                bool isAdmin = await _dbContext.TeamMembers
                    .AnyAsync(m => m.TeamId == request.TeamId &&
                                   m.UserId == request.UserId &&
                                   (m.IsAdmin || m.IsRoot) &&
                                   !m.IsDeleted,
                        cancellationToken);

                _logger.LogInformation("用户 {UserId} {Result} 团队 {TeamId} 的管理员",
                    request.UserId, isAdmin ? "是" : "不是", request.TeamId);

                return isAdmin;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查用户是否是团队管理员失败: TeamId={TeamId}, UserId={UserId}, Error={Error}",
                    request.TeamId, request.UserId, ex.Message);
                throw;
            }
        }
    }
}