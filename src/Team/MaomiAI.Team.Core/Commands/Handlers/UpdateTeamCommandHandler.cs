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

            var anySameName = await _dbContext.Teams.Where(x => x.Id != request.Id && x.Name == request.Name).AnyAsync();
            if (anySameName)
            {
                throw new BusinessException("已存在相同名称的团队");
            }

            team.Name = request.Name;
            team.Description = request.Description;
            team.AvatarFileId = request.Avatar;
            team.IsPublic = request.IsPublic;
            team.IsDisable = request.IsDisable;
            team.Markdown = request.Markdown ?? string.Empty;

            _dbContext.Update(team);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}