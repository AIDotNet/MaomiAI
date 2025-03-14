// <copyright file="JwtMiddleware.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.Security.Claims;
using MaomiAI.Services;
using Microsoft.AspNetCore.Authorization;

namespace MaomiAI.Middlewares;

/// <summary>
/// JWT中间件.
/// </summary>
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtMiddleware"/> class.
    /// </summary>
    /// <param name="next">请求委托.</param>
    /// <param name="logger">日志记录器.</param>
    public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// 处理HTTP请求.
    /// </summary>
    /// <param name="context">HTTP上下文.</param>
    /// <param name="jwtService">JWT服务.</param>
    /// <returns>异步任务.</returns>
    public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
    {
        // 检查是否有AllowAnonymous特性
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            await _next(context);
            return;
        }

        var token = GetTokenFromRequest(context);
        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                // 验证令牌并设置HttpContext.User
                AttachUserToContext(context, jwtService, token);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "JWT令牌验证失败");
            }
        }

        await _next(context);
    }

    private static string? GetTokenFromRequest(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authHeader.Substring(7);
        }

        return null;
    }

    private static void AttachUserToContext(HttpContext context, IJwtService jwtService, string token)
    {
        if (jwtService.ValidateToken(token))
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var claims = new List<Claim>();

            // 添加用户ID
            var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userIdClaim.Value));
            }

            // 添加用户名
            var nameClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (nameClaim != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));
            }

            // 添加角色
            var roleClaims = jwtToken.Claims.Where(x => x.Type == ClaimTypes.Role);
            foreach (var roleClaim in roleClaims)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleClaim.Value));
            }

            var identity = new ClaimsIdentity(claims, "Bearer");
            context.User = new ClaimsPrincipal(identity);
        }
    }
}