// <copyright file="ChangePasswordCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.User.Core.Services;
using MaomiAI.User.Shared;
using MaomiAI.User.Shared.Commands;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.User.Core.Commands.Handlers;

/// <summary>
/// 修改密码命令处理程序.
/// </summary>
public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly MaomiaiContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangePasswordCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    public ChangePasswordCommandHandler(MaomiaiContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 处理修改密码命令.
    /// </summary>
    /// <param name="request">命令请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>Task.</returns>
    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
                              .Where(u => u.Id == request.UserId && !u.IsDeleted)
                              .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new InvalidOperationException($"用户 {request.UserId} 不存在或已被删除");
        }

        // 验证旧密码
        if (!VerifyPassword(request.OldPassword, user.Password))
        {
            throw new InvalidOperationException("旧密码不正确");
        }

        // 新密码不能与旧密码相同
        if (request.OldPassword == request.NewPassword)
        {
            throw new InvalidOperationException("新密码不能与旧密码相同");
        }

        // 更新密码
        user.Password = HashPassword(request.NewPassword);
        user.UpdateTime = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
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

    // 验证密码
    private static bool VerifyPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split(':');
        if (parts.Length != 2)
        {
            return false;
        }

        var hash = parts[0];
        var salt = parts[1];
        var computedHash = HashHelper.ComputeSha256Hash(password + salt);

        return computedHash == hash;
    }
} 