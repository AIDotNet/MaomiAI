// <copyright file="CreateUserCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.User.Core.Services;
using MaomiAI.User.Shared;
using MaomiAI.User.Shared.Commands;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace MaomiAI.User.Core.Commands.Handlers;

/// <summary>
/// 创建用户命令处理程序.
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly MaomiaiContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    public CreateUserCommandHandler(MaomiaiContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 处理创建用户命令.
    /// </summary>
    /// <param name="request">命令请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>新用户ID.</returns>
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // 检查用户名是否已存在
        var usernameExists = await _dbContext.User
                                       .AnyAsync(u => u.UserName == request.UserName && !u.IsDeleted, cancellationToken);
        if (usernameExists)
        {
            throw new InvalidOperationException($"用户名 '{request.UserName}' 已被使用");
        }

        // 检查邮箱是否已存在
        var emailExists = await _dbContext.User
                                    .AnyAsync(u => u.Email == request.Email && !u.IsDeleted, cancellationToken);
        if (emailExists)
        {
            throw new InvalidOperationException($"邮箱 '{request.Email}' 已被使用");
        }

        var user = new UserEntity
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            Email = request.Email,
            Password = HashPassword(request.Password),
            NickName = request.NickName,
            AvatarUrl = request.AvatarUrl ?? string.Empty,
            Phone = request.Phone ?? string.Empty,
            Status = request.Status,
            CreateTime = DateTimeOffset.UtcNow,
            UpdateTime = DateTimeOffset.UtcNow,
            Extensions = "{}"
        };

        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    // 哈希密码
    private static string HashPassword(string password)
    {
        // 在实际应用中，应该使用更安全的密码哈希算法，例如BCrypt或Argon2
        // 这里为了简化，使用简单的SHA256+Salt
        var salt = Guid.NewGuid().ToString("N");
        var hash = HashHelper.ComputeSha256Hash(password + salt);
        return $"{hash}:{salt}";
    }
} 