// <copyright file="LoginCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using MaomiAI.Database;
using MaomiAI.User.Core.Services;
using MaomiAI.User.Shared.Commands;
using MaomiAI.User.Shared.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MaomiAI.User.Core.Commands.Handlers;

/// <summary>
/// 登录命令处理程序.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly MaomiaiContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginCommandHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="configuration">配置.</param>
    /// <param name="logger">日志记录器.</param>
    public LoginCommandHandler(MaomiaiContext dbContext, IConfiguration configuration, ILogger<LoginCommandHandler> logger)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// 处理登录命令.
    /// </summary>
    /// <param name="request">命令请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>登录结果.</returns>
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
            .Where(u => u.UserName == request.Username || u.Phone == request.Username || u.Email == request.Username)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException("用户名或密码错误");

        if (!PasswordService.VerifyPassword(request.Password, user.Password))
        {
            _logger.LogWarning("密码验证失败: {Username}", request.Username);
            throw new InvalidOperationException("用户名或密码错误");
        }

        if (!user.Status)
        {
            _logger.LogWarning("禁用用户尝试登录: {Username}", request.Username);
            throw new InvalidOperationException("用户已被禁用");
        }

        var result = GenerateAccessToken(user);
        _logger.LogInformation("用户登录成功: {Username}, ID: {UserId}", user.UserName, user.Id);

        return result;
    }

    // 生成访问令牌
    private LoginResult GenerateAccessToken(Database.Entities.UserEntity user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"] ?? "MaomiAIDefaultSecretKey1234567890ABCDEFGH";
        var issuer = jwtSettings["Issuer"] ?? "MaomiAI";
        var audience = jwtSettings["Audience"] ?? "MaomiAPIClient";
        var expirationInMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "1440"); // 默认24小时

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.NickName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("username", user.UserName),
        };

        var expirationTime = DateTimeOffset.UtcNow.AddMinutes(expirationInMinutes).ToUnixTimeMilliseconds();

        var token = new JwtSecurityToken(
            issuer: issuer,
            claims: claims,
            audience: audience,
            signingCredentials: credentials,
            expires: DateTimeOffset.FromUnixTimeMilliseconds(expirationTime).UtcDateTime);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(token);

        return new LoginResult
        {
            UserId = user.Id,
            UserName = user.UserName,
            AccessToken = tokenString,
            ExpiresIn = expirationTime
        };
    }
}