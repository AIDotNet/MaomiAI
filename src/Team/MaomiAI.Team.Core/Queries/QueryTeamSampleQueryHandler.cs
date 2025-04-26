// <copyright file="QueryTeamSampleQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.AI.Exceptions;
using MaomiAI.Database;
using MaomiAI.Team.Shared.Models;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.Team.Core.Queries;

/// <summary>
/// 获取团队详细信息.
/// </summary>
public class QueryTeamSampleQueryHandler : IRequestHandler<TeamSimpleQuery, UserJoinedTeamItemResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<QueryTeamDetailQueryHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryTeamSampleQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    public QueryTeamSampleQueryHandler(DatabaseContext dbContext, ILogger<QueryTeamDetailQueryHandler> logger)
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
    public async Task<UserJoinedTeamItemResponse> Handle(TeamSimpleQuery request, CancellationToken cancellationToken)
    {
        var team = await _dbContext.Teams.Where(t => t.Id == request.Id)
            .Select(x => new UserJoinedTeamItemResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                AvatarFileId = x.AvatarFileId,
                IsDisable = x.IsDisable,
                CreateTime = x.CreateTime,
                UpdateTime = x.UpdateTime,
                CreateUserId = x.CreateUserId,
                IsPublic = x.IsPublic,
                OwnUserId = x.OwnerId,
                OwnUserName = string.Empty
            }).FirstOrDefaultAsync(cancellationToken);

        if (team == null)
        {
            throw new BusinessException("未找到团队");
        }

        return team;
    }
}