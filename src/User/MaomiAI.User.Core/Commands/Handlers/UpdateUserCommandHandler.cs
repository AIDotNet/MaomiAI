// <copyright file="UpdateUserCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Models;
using MaomiAI.User.Shared;
using MaomiAI.User.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.User.Core.Commands.Handlers;

/// <summary>
/// 更新用户命令处理程序.
/// </summary>
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, EmptyDto>
{
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    public UpdateUserCommandHandler(MaomiaiContext dbContext, ILogger<UpdateUserCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// 处理更新用户命令.
    /// </summary>
    /// <param name="request">命令请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>Task.</returns>
    public async Task<EmptyDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Where(u => u.Id == request.Id && !u.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new InvalidOperationException($"用户 {request.Id} 不存在或已被删除");
        }

        // 检查邮箱是否已被其他用户使用
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            var emailExists = await _dbContext.Users
                .AnyAsync(u => u.Email == request.Email && u.Id != request.Id && !u.IsDeleted, cancellationToken);
            if (emailExists)
            {
                throw new InvalidOperationException($"邮箱 '{request.Email}' 已被其他用户使用");
            }
        }

        // 使用实体的Update方法更新用户信息
        user.Update(
            email: request.Email,
            nickName: request.NickName,
            avatarUrl: request.AvatarUrl,
            phone: request.Phone);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("用户更新成功: ID: {UserId}", request.Id);
        return new EmptyDto();
    }
}