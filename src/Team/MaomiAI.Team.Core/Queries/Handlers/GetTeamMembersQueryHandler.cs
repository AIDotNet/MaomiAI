// <copyright file="GetTeamMembersQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Team.Shared.Models;
using MaomiAI.Team.Shared.Queries;
using MaomiAI.User.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Queries.Handlers
{
    /// <summary>
    /// 处理获取团队成员列表查询.
    /// </summary>
    public class GetTeamMembersQueryHandler : IRequestHandler<GetTeamMembersQuery, PagedResult<TeamMemberDto>>
    {
        private readonly MaomiaiContext _dbContext;
        private readonly ILogger<GetTeamMembersQueryHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTeamMembersQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        /// <param name="logger">日志记录器.</param>
        public GetTeamMembersQueryHandler(MaomiaiContext dbContext, ILogger<GetTeamMembersQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// 处理获取团队成员列表查询.
        /// </summary>
        /// <param name="request">查询请求.</param>
        /// <param name="cancellationToken">取消令牌.</param>
        /// <returns>团队成员列表.</returns>
        /// <exception cref="InvalidOperationException">当团队不存在时抛出.</exception>
        public async Task<PagedResult<TeamMemberDto>> Handle(GetTeamMembersQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                // 验证团队是否存在
                TeamEntity? team = await _dbContext.Teams
                    .FirstOrDefaultAsync(t => t.Uuid == request.TeamId && !t.IsDeleted, cancellationToken);

                if (team == null)
                {
                    _logger.LogWarning("尝试获取不存在的团队的成员列表: {TeamId}", request.TeamId);
                    throw new InvalidOperationException($"ID为{request.TeamId}的团队不存在");
                }

                // 构建基础查询
                var query = from member in _dbContext.TeamMembers
                    join user in _dbContext.Users on member.UserId equals user.Id
                    where member.TeamId == request.TeamId && !member.IsDeleted
                    select new { Member = member, User = user };

                // 应用筛选条件
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    string keyword = "%" + request.Keyword.ToUpperInvariant() + "%";
                    query = query.Where(x => EF.Functions.ILike(x.User.UserName, keyword));
                }

                if (request.IsAdmin.HasValue)
                {
                    query = query.Where(x => x.Member.IsAdmin == request.IsAdmin.Value);
                }

                // 计算总数
                int totalCount = await query.CountAsync(cancellationToken);

                // 如果没有数据，直接返回空结果
                if (totalCount == 0)
                {
                    _logger.LogInformation(
                        "查询团队 {TeamId} 的成员列表结果为空，条件: Keyword={Keyword}, IsAdmin={IsAdmin}, Page={Page}, PageSize={PageSize}",
                        request.TeamId, request.Keyword, request.IsAdmin, request.Page, request.PageSize);
                    return new PagedResult<TeamMemberDto>(new List<TeamMemberDto>(), 0, request.Page, request.PageSize);
                }

                // 应用排序和分页，并映射到DTO
                List<TeamMemberDto>? items = await query
                    .OrderByDescending(x => x.Member.IsRoot)
                    .ThenByDescending(x => x.Member.IsAdmin)
                    .ThenByDescending(x => x.Member.CreateTime)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new TeamMemberDto
                    {
                        Id = x.Member.Id,
                        TeamId = x.Member.TeamId,
                        UserId = x.Member.UserId,
                        UserName = x.User.UserName,
                        UserAvatar = x.User.AvatarUrl,
                        IsRoot = x.Member.IsRoot,
                        IsAdmin = x.Member.IsAdmin,
                        CreateTime = x.Member.CreateTime
                    })
                    .ToListAsync(cancellationToken);

                _logger.LogInformation(
                    "查询团队 {TeamId} 的成员列表成功，条件: Keyword={Keyword}, IsAdmin={IsAdmin}, Page={Page}, PageSize={PageSize}, 总数={TotalCount}, 返回数量={Count}",
                    request.TeamId, request.Keyword, request.IsAdmin, request.Page, request.PageSize, totalCount,
                    items.Count);

                return new PagedResult<TeamMemberDto>(items, totalCount, request.Page, request.PageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询团队 {TeamId} 的成员列表失败: {Message}", request.TeamId, ex.Message);
                throw;
            }
        }
    }
}