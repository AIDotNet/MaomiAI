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

namespace MaomiAI.User.Core.Commands.Handlers;

/// <summary>
/// 切换用户状态命令处理程序.
/// </summary>
public class ToggleUserStatusCommandHandler : IRequestHandler<ToggleUserStatusCommand>
{
    private readonly MaomiaiContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleUserStatusCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    public ToggleUserStatusCommandHandler(MaomiaiContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 处理切换用户状态命令.
    /// </summary>
    /// <param name="request">命令请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>Task.</returns>
    public async Task Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
                              .Where(u => u.Id == request.UserId && !u.IsDeleted)
                              .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new InvalidOperationException($"用户 {request.UserId} 不存在或已被删除");
        }

        user.Status = request.Status;
        user.UpdateTime = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
} 