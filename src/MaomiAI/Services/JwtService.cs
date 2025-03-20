// <copyright file="JwtService.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MaomiAI.Services
{
    /// <summary>
    /// JWT服务接口.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// 生成JWT令牌.
        /// </summary>
        /// <param name="userId">用户ID.</param>
        /// <param name="userName">用户名.</param>
        /// <param name="roles">角色列表.</param>
        /// <returns>JWT令牌.</returns>
        string GenerateToken(Guid userId, string userName, IEnumerable<string>? roles = null);

        /// <summary>
        /// 验证JWT令牌.
        /// </summary>
        /// <param name="token">JWT令牌.</param>
        /// <returns>如果令牌有效则返回true，否则返回false.</returns>
        bool ValidateToken(string token);

        /// <summary>
        /// 从JWT令牌中获取用户ID.
        /// </summary>
        /// <param name="token">JWT令牌.</param>
        /// <returns>用户ID.</returns>
        Guid? GetUserIdFromToken(string token);
    }

    /// <summary>
    /// JWT服务实现.
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtService"/> class.
        /// </summary>
        /// <param name="jwtOptions">JWT配置选项.</param>
        public JwtService(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        /// <inheritdoc/>
        public string GenerateToken(Guid userId, string userName, IEnumerable<string>? roles = null)
        {
            List<Claim>? claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, userName)
            };

            // 添加角色声明
            if (roles != null)
            {
                foreach (string? role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            SymmetricSecurityKey? key = new(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            SigningCredentials? creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken? token = new(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <inheritdoc/>
        public bool ValidateToken(string token)
        {
            JwtSecurityTokenHandler? tokenHandler = new();
            byte[]? key = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public Guid? GetUserIdFromToken(string token)
        {
            JwtSecurityTokenHandler? tokenHandler = new();
            byte[]? key = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);

            try
            {
                ClaimsPrincipal? principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                Claim? userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return userId;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}