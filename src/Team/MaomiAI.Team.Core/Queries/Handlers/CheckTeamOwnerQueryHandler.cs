// <copyright file="CheckTeamOwnerQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Team.Shared.Queries;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Queries.Handlers;

/// <summary>
/// 处理检查用户是否是团队所有者的查询.
/// </summary>
public class CheckTeamOwnerQueryHandler : IRequestHandler<CheckTeamOwnerQuery, bool>
{
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<CheckTeamOwnerQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckTeamOwnerQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    public CheckTeamOwnerQueryHandler(MaomiaiContext dbContext, ILogger<CheckTeamOwnerQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// 处理检查用户是否是团队所有者的查询.
    /// </summary>
    /// <param name="request">查询请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>如果用户是团队所有者则返回true，否则返回false.</returns>
    public async Task<bool> Handle(CheckTeamOwnerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("检查用户是否是团队所有者: TeamId={TeamId}, UserId={UserId}", request.TeamId, request.UserId);

            // 首先检查团队是否存在
            var teamExists = await _dbContext.Teams
                .AnyAsync(t => t.Uuid == request.TeamId && !t.IsDeleted, cancellationToken);

            if (!teamExists)
            {
                _logger.LogWarning("团队不存在: TeamId={TeamId}", request.TeamId);
                return false;
            }

            // 检查用户是否是团队所有者
            var isOwner = await _dbContext.TeamMembers
                .AnyAsync(m => m.TeamId == request.TeamId &&
                               m.UserId == request.UserId &&
                               m.IsRoot &&
                               !m.IsDeleted,
                         cancellationToken);

            _logger.LogInformation("用户 {UserId} {Result} 团队 {TeamId} 的所有者",
                request.UserId, isOwner ? "是" : "不是", request.TeamId);

            return isOwner;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "检查用户是否是团队所有者失败: TeamId={TeamId}, UserId={UserId}, Error={Error}",
                request.TeamId, request.UserId, ex.Message);
            throw;
        }
    }
}
