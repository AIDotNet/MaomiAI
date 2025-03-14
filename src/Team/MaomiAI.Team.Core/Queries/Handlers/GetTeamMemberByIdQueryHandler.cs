// <copyright file="GetTeamMemberByIdQueryHandler.cs" company="MaomiAI">
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

namespace MaomiAI.Team.Core.Queries.Handlers;

/// <summary>
/// 处理根据ID获取团队成员查询.
/// </summary>
public class GetTeamMemberByIdQueryHandler : IRequestHandler<GetTeamMemberByIdQuery, TeamMemberDto?>
{
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<GetTeamMemberByIdQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTeamMemberByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    public GetTeamMemberByIdQueryHandler(MaomiaiContext dbContext, ILogger<GetTeamMemberByIdQueryHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// 处理根据ID获取团队成员查询.
    /// </summary>
    /// <param name="request">查询请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>团队成员信息，如果不存在则返回null.</returns>
    public async Task<TeamMemberDto?> Handle(GetTeamMemberByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("获取团队成员信息: TeamId={TeamId}, UserId={UserId}", request.TeamId, request.UserId);

            // 查询团队成员信息
            var result = await (from member in _dbContext.TeamMembers
                                join user in _dbContext.Users on member.UserId equals user.Id
                                where member.TeamId == request.TeamId &&
                                      member.UserId == request.UserId &&
                                      !member.IsDeleted
                                select new TeamMemberDto
                                {
                                    Id = member.Id,
                                    TeamId = member.TeamId,
                                    UserId = member.UserId,
                                    UserName = user.UserName,
                                    UserAvatar = user.AvatarUrl,
                                    IsRoot = member.IsRoot,
                                    IsAdmin = member.IsAdmin,
                                    CreateTime = member.CreateTime
                                })
                              .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                _logger.LogWarning("未找到团队成员: TeamId={TeamId}, UserId={UserId}", request.TeamId, request.UserId);
            }
            else
            {
                _logger.LogInformation("成功获取团队成员信息: TeamId={TeamId}, UserId={UserId}, IsAdmin={IsAdmin}, IsRoot={IsRoot}",
                    request.TeamId, request.UserId, result.IsAdmin, result.IsRoot);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取团队成员信息失败: TeamId={TeamId}, UserId={UserId}, Error={Error}",
                request.TeamId, request.UserId, ex.Message);
            throw;
        }
    }
}
