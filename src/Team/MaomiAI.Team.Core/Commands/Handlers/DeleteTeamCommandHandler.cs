// <copyright file="DeleteTeamCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.AI.Exceptions;
using MaomiAI.Database;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Commands.Handlers
{
    /// <summary>
    /// 处理删除团队命令.
    /// </summary>
    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand>
    {
        private readonly MaomiaiContext _dbContext;
        private readonly ILogger<DeleteTeamCommandHandler> _logger;
        private readonly UserContext _userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteTeamCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        /// <param name="logger">日志记录器.</param>
        /// <param name="userContext">用户上下文.</param>
        public DeleteTeamCommandHandler(
            MaomiaiContext dbContext,
            ILogger<DeleteTeamCommandHandler> logger,
            UserContext userContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userContext = userContext;
        }

        /// <summary>
        /// 处理删除团队命令.
        /// </summary>
        /// <param name="request">删除团队命令.</param>
        /// <param name="cancellationToken">取消令牌.</param>
        /// <returns>任务.</returns>
        /// <exception cref="InvalidOperationException">当团队不存在时抛出.</exception>
        public async Task Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _dbContext.Teams
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (team == null)
            {
                throw new BusinessException("团队不存在.");
            }

            if (team.OwnId != _userContext.UserId)
            {
                throw new BusinessException("没有权限删除该团队.");
            }

            // todo: 检查团队下是否有资源
            _dbContext.Teams.Remove(team);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}