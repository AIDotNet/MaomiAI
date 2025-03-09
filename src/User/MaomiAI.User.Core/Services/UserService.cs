// <copyright file="UserService.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.User.Shared;
using MaomiAI.User.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MaomiAI.User.Core.Services;

/// <summary>
/// 用户服务实现.
/// </summary>
public class UserService : IUserService
{
    private readonly MaomiaiContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    /// <param name="configuration">配置.</param>
    /// <param name="logger">日志.</param>
    public UserService(MaomiaiContext dbContext, IConfiguration configuration, ILogger<UserService> logger)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<PagedResult<UserDto>> GetUsersAsync(int page, int pageSize, string? keyword, bool? status, CancellationToken cancellationToken)
    {
        IQueryable<UserEntity> query = _dbContext.User.Where(u => !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim().ToLower();
            query = query.Where(u => u.UserName.ToLower().Contains(keyword) ||
                                    u.Email.ToLower().Contains(keyword) ||
                                    u.NickName.ToLower().Contains(keyword));
        }

        if (status.HasValue)
        {
            query = query.Where(u => u.Status == status.Value);
        }

        var totalCount = await query.LongCountAsync(cancellationToken);
        var items = await query.OrderByDescending(u => u.CreateTime)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .Select(u => new UserDto
                              {
                                  Id = u.Id,
                                  UserName = u.UserName,
                                  Email = u.Email,
                                  NickName = u.NickName,
                                  AvatarUrl = u.AvatarUrl,
                                  Phone = u.Phone,
                                  Status = u.Status,
                                  CreateTime = u.CreateTime,
                                  UpdateTime = u.UpdateTime
                              })
                              .ToListAsync(cancellationToken);

        return new PagedResult<UserDto>(items, totalCount, page, pageSize);
    }

    /// <inheritdoc/>
    public async Task<UserDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
                                  .Where(u => u.Id == id && !u.IsDeleted)
                                  .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            NickName = user.NickName,
            AvatarUrl = user.AvatarUrl,
            Phone = user.Phone,
            Status = user.Status,
            CreateTime = user.CreateTime,
            UpdateTime = user.UpdateTime
        };
    }

    /// <inheritdoc/>
    public async Task<Guid> CreateUserAsync(UserEntity entity, CancellationToken cancellationToken)
    {
        // 检查用户名是否已存在
        var usernameExists = await _dbContext.User
                                           .AnyAsync(u => u.UserName == entity.UserName && !u.IsDeleted, cancellationToken);
        if (usernameExists)
        {
            throw new InvalidOperationException($"用户名 '{entity.UserName}' 已被使用");
        }

        // 检查邮箱是否已存在
        var emailExists = await _dbContext.User
                                        .AnyAsync(u => u.Email == entity.Email && !u.IsDeleted, cancellationToken);
        if (emailExists)
        {
            throw new InvalidOperationException($"邮箱 '{entity.Email}' 已被使用");
        }

        // 密码加密
        entity.Password = HashPassword(entity.Password);
        entity.Id = Guid.NewGuid();
        entity.CreateTime = DateTimeOffset.UtcNow;
        entity.UpdateTime = DateTimeOffset.UtcNow;
        entity.Extensions = "{}";

        _dbContext.User.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateUserAsync(Guid id, Action<UserEntity> updateAction, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
                                  .Where(u => u.Id == id && !u.IsDeleted)
                                  .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return false;
        }

        updateAction(user);
        user.UpdateTime = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
                                  .Where(u => u.Id == id && !u.IsDeleted)
                                  .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return false;
        }

        user.IsDeleted = true;
        user.UpdateTime = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc/>
    public async Task<UserEntity?> ValidateUserPasswordAsync(string usernameOrEmail, string password, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
                                  .Where(u => !u.IsDeleted &&
                                             (u.UserName == usernameOrEmail || u.Email == usernameOrEmail))
                                  .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return null;
        }

        // 验证密码
        if (!VerifyPassword(password, user.Password))
        {
            return null;
        }

        // 检查用户状态
        if (!user.Status)
        {
            throw new InvalidOperationException("用户已被禁用");
        }

        return user;
    }

    /// <inheritdoc/>
    public async Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
                                  .Where(u => u.Id == userId && !u.IsDeleted)
                                  .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return false;
        }

        // 验证旧密码
        if (!VerifyPassword(oldPassword, user.Password))
        {
            throw new InvalidOperationException("旧密码不正确");
        }

        // 新密码不能与旧密码相同
        if (oldPassword == newPassword)
        {
            throw new InvalidOperationException("新密码不能与旧密码相同");
        }

        // 更新密码
        user.Password = HashPassword(newPassword);
        user.UpdateTime = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> ToggleUserStatusAsync(Guid userId, bool status, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
                                  .Where(u => u.Id == userId && !u.IsDeleted)
                                  .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return false;
        }

        user.Status = status;
        user.UpdateTime = DateTimeOffset.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc/>
    public LoginResult GenerateAccessToken(UserEntity user)
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