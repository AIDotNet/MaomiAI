// <copyright file="UpdateUserCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.User.Shared;
using MaomiAI.User.Shared.Commands;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace MaomiAI.User.Core.Commands.Handlers;

/// <summary>
/// 更新用户命令处理程序.
/// </summary>
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly MaomiaiContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    public UpdateUserCommandHandler(MaomiaiContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 处理更新用户命令.
    /// </summary>
    /// <param name="request">命令请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>Task.</returns>
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
                               .Where(u => u.Id == request.Id && !u.IsDeleted)
                               .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new InvalidOperationException($"用户 {request.Id} 不存在或已被删除");
        }

        // 检查邮箱是否已被其他用户使用
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            var emailExists = await _dbContext.User
                                        .AnyAsync(u => u.Email == request.Email && u.Id != request.Id && !u.IsDeleted, cancellationToken);
            if (emailExists)
            {
                throw new InvalidOperationException($"邮箱 '{request.Email}' 已被其他用户使用");
            }

            user.Email = request.Email;
        }

        if (!string.IsNullOrEmpty(request.NickName))
        {
            user.NickName = request.NickName;
        }

        if (!string.IsNullOrEmpty(request.AvatarUrl))
        {
            user.AvatarUrl = request.AvatarUrl;
        }

        if (!string.IsNullOrEmpty(request.Phone))
        {
            user.Phone = request.Phone;
        }

        user.UpdateTime = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
} 