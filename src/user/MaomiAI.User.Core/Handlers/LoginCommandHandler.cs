// <copyright file="LoginCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.AI.Exceptions;
using MaomiAI.Database;
using MaomiAI.Infra;
using MaomiAI.Infra.Helpers;
using MaomiAI.Infra.Services;
using MaomiAI.User.Core.Services;
using MaomiAI.User.Shared.Commands;
using MaomiAI.User.Shared.Commands.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MaomiAI.User.Core.Handlers;

/// <summary>
/// 登录命令处理程序.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly DatabaseContext _dbContext;
    private readonly SystemOptions _systemOptions;
    private readonly IRedisDatabase _database;
    private readonly IRsaProvider _rsaProvider;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<LoginCommandHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="systemOptions"></param>
    /// <param name="database"></param>
    /// <param name="rsaProvider"></param>
    /// <param name="tokenProvider"></param>
    /// <param name="logger"></param>
    public LoginCommandHandler(DatabaseContext dbContext, SystemOptions systemOptions, IRedisDatabase database, IRsaProvider rsaProvider, ITokenProvider tokenProvider, ILogger<LoginCommandHandler> logger)
    {
        _dbContext = dbContext;
        _systemOptions = systemOptions;
        _database = database;
        _rsaProvider = rsaProvider;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }

    /// <summary>
    /// 处理登录命令.
    /// </summary>
    /// <param name="request">命令请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>登录结果.</returns>
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"login:fail:{request.UserName}";
        var failCount = await _database.GetAsync<int>(cacheKey);

        if (failCount >= 5)
        {
            throw new BusinessException("登录失败次数过多，请稍后再试.") { StatusCode = 403 };
        }

        var user = await _dbContext.Users.Where(u =>
                                  u.UserName == request.UserName || u.Email == request.UserName)
                              .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            await IncrementLoginFailCountAsync(cacheKey);
            throw new BusinessException("用户名或密码错误") { StatusCode = 401 };
        }

        if (!user.IsEnable)
        {
            throw new BusinessException("用户已被禁用") { StatusCode = 401 };
        }

        try
        {
            var password = _rsaProvider.Decrypt(request.Password);
            if (!PBKDF2Helper.VerifyHash(password, user.Password, user.PasswordHalt))
            {
                await IncrementLoginFailCountAsync(cacheKey);
                throw new BusinessException("用户名或密码错误") { StatusCode = 401 };
            }
        }
        catch (Exception ex) when (ex is not BusinessException)
        {
            await IncrementLoginFailCountAsync(cacheKey);
            throw new BusinessException("用户名或密码错误") { StatusCode = 401 };
        }

        // 登录成功，清除失败计数
        await _database.Database.KeyDeleteAsync(cacheKey);

        var userContext = new DefaultUserContext
        {
            UserId = user.Id,
            UserName = user.UserName,
            NickName = user.NickName,
            Email = user.Email
        };

        var (accessToken, refreshToken) = _tokenProvider.GenerateTokens(userContext);

        var result = new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserId = user.Id,
            UserName = user.UserName,
            ExpiresIn = DateTimeOffset.Now.AddMinutes(30).ToUnixTimeMilliseconds()
        };

        _logger.LogInformation("User login.{@Message}", new { user.Id, user.UserName, user.NickName });

        return result;
    }

    /// <summary>
    /// 增加登录失败计数.
    /// </summary>
    /// <param name="cacheKey">缓存键.</param>
    /// <returns>异步任务.</returns>
    private async Task IncrementLoginFailCountAsync(string cacheKey)
    {
        var failCount = await _database.Database.StringIncrementAsync(cacheKey);
        await _database.Database.KeyExpireAsync(cacheKey, TimeSpan.FromMinutes(5));
    }
}