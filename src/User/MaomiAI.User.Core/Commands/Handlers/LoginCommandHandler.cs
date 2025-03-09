// <copyright file="LoginCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.User.Core.Services;
using MaomiAI.User.Shared;
using MaomiAI.User.Shared.Commands;
using MaomiAI.User.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MaomiAI.User.Core.Commands.Handlers;

/// <summary>
/// 登录命令处理程序.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly MaomiaiContext _dbContext;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="configuration">配置.</param>
    public LoginCommandHandler(MaomiaiContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
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
                                .Where(u => !u.IsDeleted && 
                                           (u.UserName == request.Username || u.Email == request.Username))
                                .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new InvalidOperationException("用户名或密码错误");
        }

        // 验证密码
        if (!VerifyPassword(request.Password, user.Password))
        {
            throw new InvalidOperationException("用户名或密码错误");
        }

        // 检查用户状态
        if (!user.Status)
        {
            throw new InvalidOperationException("用户已被禁用");
        }

        return GenerateAccessToken(user);
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

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
            signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(token);

        return new LoginResult
        {
            UserId = user.Id,
            UserName = user.UserName,
            AccessToken = tokenString,
            ExpiresIn = expirationInMinutes * 60
        };
    }
} 