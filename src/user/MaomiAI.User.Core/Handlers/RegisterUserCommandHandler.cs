// <copyrig   1ht file="RegisterUserCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.AI.Exceptions;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Helpers;
using MaomiAI.Infra.Models;
using MaomiAI.Infra.Services;
using MaomiAI.User.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.User.Core.Handlers;

/// <summary>
/// 注册账号.
/// </summary>
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, GuidResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<RegisterUserCommandHandler> _logger;
    private readonly IRsaProvider _rsaProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterUserCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    /// <param name="rsaProvider"></param>
    public RegisterUserCommandHandler(DatabaseContext dbContext, ILogger<RegisterUserCommandHandler> logger, IRsaProvider rsaProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
        _rsaProvider = rsaProvider;
    }

    /// <summary>
    /// 处理创建用户命令.
    /// </summary>
    /// <param name="request">命令请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>新用户ID.</returns>
    public async Task<GuidResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users
                .Where(u => u.UserName == request.UserName || u.Email == request.Email || u.Phone == request.Phone)
                .Select(u => new { u.UserName, u.Email, u.Phone })
                .FirstOrDefaultAsync();

        if (existingUser != null)
        {
            if (existingUser.UserName == request.UserName)
            {
                throw new BusinessException("用户名 {0} 已被注册", request.UserName) { StatusCode = 409 };
            }

            if (existingUser.Email == request.Email)
            {
                throw new BusinessException("邮箱 {0} 已被注册", request.Email) { StatusCode = 409 };
            }

            if (existingUser.Phone == request.Phone)
            {
                throw new BusinessException("手机号 {0} 已被注册", request.Phone) { StatusCode = 409 };
            }
        }

        // 使用 RSA 解密还原密码
        string restorePassword = default!;
        try
        {
            restorePassword = _rsaProvider.Decrypt(request.Password);
        }
        catch
        {
            throw new BusinessException("密码验证失败") { StatusCode = 400 };
        }

        try
        {
            // 使用 PBKDF2 算法生成哈希值
            var (hashedPassword, saltBase64) = PBKDF2Helper.ToHash(restorePassword);
            var userEntity = new UserEntity
            {
                Email = request.Email,
                NickName = request.NickName,
                UserName = request.UserName,
                Password = hashedPassword,
                PasswordHalt = saltBase64,
                AvatarUrl = string.Empty,
                Phone = request.Phone,
                IsEnable = true
            };

            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new GuidResponse
            {
                Guid = userEntity.Id
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Account registration failed.{@Command}", request);
            throw new BusinessException("账号注册失败");
        }
    }
}