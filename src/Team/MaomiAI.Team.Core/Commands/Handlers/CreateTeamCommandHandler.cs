// <copyright file="CreateTeamCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Commands;

using MediatR;

using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Commands.Handlers;

/// <summary>
/// 处理创建团队命令.
/// </summary>
public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, Guid>
{
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<CreateTeamCommandHandler> _logger;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTeamCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    /// <param name="userContext">用户上下文.</param>
    public CreateTeamCommandHandler(
        MaomiaiContext dbContext,
        ILogger<CreateTeamCommandHandler> logger,
        UserContext userContext)
    {
        _dbContext = dbContext;
        _logger = logger;
        _userContext = userContext;
    }

    /// <summary>
    /// 处理创建团队命令.
    /// </summary>
    /// <param name="request">创建团队命令.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>新创建的团队ID.</returns>
    public async Task<Guid> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _userContext.UserId;

        // 使用领域模型的工厂方法创建团队实体
        var team = TeamEntity.Create(
            request.Name,
            request.Description,
            request.Avatar,
            currentUserId);

        // 添加到数据库
        await _dbContext.Teams.AddAsync(team, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("成功创建团队: {TeamId}, 名称: {Name}, 创建者: {CreateUserId}", team.Uuid, team.Name, currentUserId);
        return team.Uuid;
    }
}