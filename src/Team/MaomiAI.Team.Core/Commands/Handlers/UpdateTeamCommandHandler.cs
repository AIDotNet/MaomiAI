// <copyright file="UpdateTeamCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.AI.Exceptions;
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
    /// 处理更新团队命令.
    /// </summary>
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand>
    {
        private readonly MaomiaiContext _dbContext;
        private readonly ILogger<UpdateTeamCommandHandler> _logger;
        private readonly UserContext _userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateTeamCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        /// <param name="logger">日志记录器.</param>
        /// <param name="userContext">用户上下文.</param>
        public UpdateTeamCommandHandler(
            MaomiaiContext dbContext,
            ILogger<UpdateTeamCommandHandler> logger,
            UserContext userContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userContext = userContext;
        }

        /// <inheritdoc/>
        public async Task Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _dbContext.Teams.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
            if (team == null)
            {
                throw new BusinessException("团队不存在");
            }


            try
            {
                // 获取当前用户ID
                Guid currentUserId = _userContext.UserId;


                if (team == null)
                {
                    _logger.LogWarning("尝试更新不存在的团队: {TeamId}", request.Id);
                    throw new InvalidOperationException($"ID为{request.Id}的团队不存在");
                }

                // 使用领域模型的方法更新团队
                team.Update(
                    request.Name,
                    request.Description,
                    request.Avatar,
                    currentUserId);

                await _dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("成功更新团队: {TeamId}, 更新者: {UpdateUserId}", team.Id, currentUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新团队失败: {TeamId}, {Message}", request.Id, ex.Message);
                throw;
            }
        }
    }
}