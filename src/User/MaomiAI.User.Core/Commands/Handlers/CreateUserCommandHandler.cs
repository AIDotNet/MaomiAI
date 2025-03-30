// <copyright file="CreateUserCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Helpers;
using MaomiAI.User.Shared.Commands;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.User.Core.Commands.Handlers;

/// <summary>
/// 创建用户命令处理程序.
/// </summary>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="logger">日志记录器.</param>
    public CreateUserCommandHandler(MaomiaiContext dbContext, ILogger<CreateUserCommandHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// 处理创建用户命令.
    /// </summary>
    /// <param name="request">命令请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>新用户ID.</returns>
    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUser = await _dbContext.Users
                .Where(u => u.UserName == request.UserName || u.Email == request.Email || u.Phone == request.Phone)
                .Select(u => new { u.UserName, u.Email, u.Phone })
                .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                if (existingUser.UserName == request.UserName)
                {
                    throw new InvalidOperationException($"用户名 '{request.UserName}' 已被使用");
                }

                if (existingUser.Email == request.Email)
                {
                    throw new InvalidOperationException($"邮箱 '{request.Email}' 已被使用");
                }

                if (existingUser.Phone == request.Phone)
                {
                    throw new InvalidOperationException($"手机号 '{request.Phone}' 已被使用");
                }
            }

            string hashedPassword = UserPasswordHelper.HashPassword(request.Password);

            var user = UserEntity.Create(
                userName: request.UserName,
                email: request.Email,
                password: hashedPassword,
                nickName: request.NickName,
                avatarUrl: request.AvatarUrl,
                phone: request.Phone);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("用户创建成功: {UserName}, ID: {UserId}", request.UserName, user.Id);

            return user.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建用户失败: {UserName}, {Message}", request.UserName, ex.Message);
            throw;
        }
    }
}