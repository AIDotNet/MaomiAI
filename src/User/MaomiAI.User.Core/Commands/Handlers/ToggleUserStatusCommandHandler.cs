// <copyright file="ToggleUserStatusCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.User.Shared;
using MaomiAI.User.Shared.Commands;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.User.Core.Commands.Handlers;

/// <summary>
/// 切换用户状态命令处理程序.
/// </summary>
public class ToggleUserStatusCommandHandler : IRequestHandler<ToggleUserStatusCommand>
{
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<ToggleUserStatusCommandHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleUserStatusCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    public ToggleUserStatusCommandHandler(MaomiaiContext dbContext, ILogger<ToggleUserStatusCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// 处理切换用户状态命令.
    /// </summary>
    /// <param name="request">命令请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>Task.</returns>
    public async Task Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        // 查询所有需要更新状态的用户
        var users = await _dbContext.Users
            .Where(u => request.UserIds.Contains(u.Id))
            .ToListAsync(cancellationToken);

        if (users.Count == 0)
        {
            _logger.LogWarning("切换用户状态失败：未找到指定的用户，UserIds: {UserIds}", string.Join(", ", request.UserIds));
            throw new InvalidOperationException("未找到指定的用户");
        }

        // 记录找到的用户数量和未找到的用户ID
        var foundUserIds = users.Select(u => u.Id).ToList();
        var notFoundUserIds = request.UserIds.Except(foundUserIds).ToList();

        if (notFoundUserIds.Count > 0)
        {
            _logger.LogWarning("部分用户未找到: {NotFoundUserIds}", string.Join(", ", notFoundUserIds));
        }

        // 批量更新用户状态
        foreach (var user in users)
        {
            user.ChangeStatus(request.Status);
        }

        // 保存更改
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("成功切换用户状态: 状态={Status}, 用户数量={Count}, UserIds={UserIds}", request.Status, users.Count, string.Join(", ", foundUserIds));
    }
}