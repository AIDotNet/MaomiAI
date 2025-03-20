// <copyright file="GetTeamByIdQueryHandler.cs" company="MaomiAI">
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
    /// 处理根据ID获取团队查询.
    /// </summary>
    public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, TeamDto?>
    {
        private readonly MaomiaiContext _dbContext;
        private readonly ILogger<GetTeamByIdQueryHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTeamByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        /// <param name="logger">日志记录器.</param>
        public GetTeamByIdQueryHandler(MaomiaiContext dbContext, ILogger<GetTeamByIdQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// 处理根据ID获取团队查询.
        /// </summary>
        /// <param name="request">查询请求.</param>
        /// <param name="cancellationToken">取消令牌.</param>
        /// <returns>团队信息.</returns>
        public async Task<TeamDto?> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                TeamDto? team = await _dbContext.Teams
                    .Where(t => t.Uuid == request.Id)
                    .Select(t => new TeamDto
                    {
                        Id = t.Uuid,
                        Name = t.Name,
                        Description = t.Description,
                        Avatar = t.Avatar,
                        Status = t.Status,
                        CreateTime = t.CreateTime,
                        UpdateTime = t.UpdateTime,
                        CreateUserId = t.CreateUserId
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (team == null)
                {
                    _logger.LogInformation("未找到ID为{Id}的团队", request.Id);
                    return null;
                }

                _logger.LogInformation("成功获取ID为{Id}的团队信息", request.Id);
                return team;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取ID为{Id}的团队信息失败: {Message}", request.Id, ex.Message);
                throw;
            }
        }
    }
}