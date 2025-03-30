// <copyright file="GetUserTeamsQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Team.Shared.Models;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Queries.Handlers
{
    /// <summary>
    /// 处理获取用户所属的所有团队查询.
    /// </summary>
    public class GetUserTeamsQueryHandler : IRequestHandler<GetUserTeamsQuery, List<TeamDto>>
    {
        private readonly MaomiaiContext _dbContext;
        private readonly ILogger<GetUserTeamsQueryHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserTeamsQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        /// <param name="logger">日志记录器.</param>
        public GetUserTeamsQueryHandler(MaomiaiContext dbContext, ILogger<GetUserTeamsQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// 处理获取用户所属的所有团队查询.
        /// </summary>
        /// <param name="request">查询请求.</param>
        /// <param name="cancellationToken">取消令牌.</param>
        /// <returns>用户所属的团队列表.</returns>
        public async Task<List<TeamDto>> Handle(GetUserTeamsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("获取用户 {UserId} 所属的团队列表, 关键词: {Keyword}, 是否管理员: {IsAdmin}, 是否所有者: {IsOwner}",
                    request.UserId, request.Keyword, request.IsAdmin, request.IsOwner);

                // 验证用户是否存在
                bool userExists = await _dbContext.Users
                    .AnyAsync(u => u.Id == request.UserId && !u.IsDeleted, cancellationToken);

                if (!userExists)
                {
                    _logger.LogWarning("用户不存在: UserId={UserId}", request.UserId);
                    return new List<TeamDto>();
                }

                // 构建基础查询
                var query = from member in _dbContext.TeamMembers
                    join team in _dbContext.Teams on member.TeamId equals team.Id
                    where member.UserId == request.UserId &&
                          !member.IsDeleted &&
                          !team.IsDeleted
                    select new { Member = member, Team = team };

                // 应用筛选条件
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    string keyword = "%" + request.Keyword.ToUpperInvariant() + "%";
                    query = query.Where(x => EF.Functions.ILike(x.Team.Name, keyword));
                }

                if (request.IsAdmin.HasValue && request.IsAdmin.Value)
                {
                    query = query.Where(x => x.Member.IsAdmin);
                }

                if (request.IsOwner.HasValue && request.IsOwner.Value)
                {
                    query = query.Where(x => x.Member.IsRoot);
                }

                // 获取结果并映射到DTO
                List<TeamDto>? teams = await query
                    .OrderByDescending(x => x.Member.IsRoot)
                    .ThenByDescending(x => x.Member.IsAdmin)
                    .ThenByDescending(x => x.Team.CreateTime)
                    .Select(x => new TeamDto
                    {
                        Id = x.Team.Id,
                        Name = x.Team.Name,
                        Description = x.Team.Description,
                        Avatar = x.Team.Avatar,
                        Status = x.Team.Status,
                        CreateTime = x.Team.CreateTime,
                        UpdateTime = x.Team.UpdateTime,
                        CreateUserId = x.Team.CreateUserId,
                        IsOwner = x.Member.IsRoot,
                        IsAdmin = x.Member.IsAdmin
                    })
                    .ToListAsync(cancellationToken);

                _logger.LogInformation("成功获取用户 {UserId} 所属的团队列表，共 {Count} 个团队",
                    request.UserId, teams.Count);

                return teams;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取用户 {UserId} 所属的团队列表失败: {Message}",
                    request.UserId, ex.Message);
                throw;
            }
        }
    }
}