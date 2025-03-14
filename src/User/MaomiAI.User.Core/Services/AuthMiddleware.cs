using System.Security.Claims;

using MaomiAI.Database;
using MaomiAI.Infra.Models;
using MaomiAI.User.Shared.Attributes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.User.Core.Services;

/// <summary>
/// 认证中间件，负责从JWT令牌中提取用户信息并填充到UserContext中.
/// </summary>
public class AuthMiddleware : IMiddleware
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DefaultUserContext _userContext;
    private readonly MaomiaiContext _dbContext;
    private readonly ILogger<AuthMiddleware> _logger;

    public AuthMiddleware(
        IHttpContextAccessor httpContextAccessor,
        DefaultUserContext userContext,
        MaomiaiContext dbContext,
        ILogger<AuthMiddleware> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _userContext = userContext;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            _userContext.Clear(); // 清除用户上下文，避免请求间数据污染

            var path = context.Request.Path.Value?.ToLower();
            if (path is not null && (path.StartsWith("/scalar") || path.StartsWith("/openapi")))
            {
                _logger.LogDebug("允许 API 测试工具路径 {Path} 匿名访问", path);
                await next(context);
                return;
            }

            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null ||
                endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                _logger.LogDebug("请求路径 {Path} 允许匿名访问", path);
                await next(context);
                return;
            }

            if (context.User.Identity?.IsAuthenticated == true)
            {
                if (!await ProcessAuthenticatedUser(context) ||
                    !await CheckAuthorizationRequirements(context, endpoint))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new { message = "没有足够的权限访问此资源" });
                    return;
                }
            }
            else
            {
                _logger.LogWarning("未认证的请求尝试访问 {Path}", path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "未认证的请求" });
                return;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理用户认证时出错");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { message = "处理认证时发生内部错误" });
        }

        await next(context);
    }

    private async Task<bool> ProcessAuthenticatedUser(HttpContext context)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return false;
        }

        var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        if (user is null)
        {
            _logger.LogWarning("找不到ID为 {UserId} 的用户", userId);
            return false;
        }

        if (!user.Status)
        {
            _logger.LogWarning("用户 {UserName} ({UserId}) 已被禁用", user.UserName, user.Id);
            return false;
        }

        _userContext
            .SetUserInfo(user.Id, user.UserName, user.NickName, user.Email, user.AvatarUrl)
            .SetAuthenticated(true)
            .AddRoles(context.User.FindAll(ClaimTypes.Role).Select(c => c.Value));

        _logger.LogInformation("用户 {UserName} ({UserId}) 已认证", user.UserName, user.Id);
        return true;
    }

    private Task<bool> CheckAuthorizationRequirements(HttpContext context, Endpoint? endpoint)
    {
        if (endpoint is null) return Task.FromResult(true);

        var rolesToCheck = new HashSet<string>();
        var authorizeAttributes = endpoint.Metadata.GetOrderedMetadata<AuthorizeAttribute>();

        foreach (var attribute in authorizeAttributes)
        {
            if (attribute is RequireRolesAttribute) continue;
            if (!string.IsNullOrEmpty(attribute.Roles))
            {
                rolesToCheck.UnionWith(attribute.Roles.Split(',').Select(r => r.Trim()));
            }
        }

        var requireRolesAttributes = endpoint.Metadata.GetOrderedMetadata<RequireRolesAttribute>();
        foreach (var attribute in requireRolesAttributes)
        {
            if (!string.IsNullOrEmpty(attribute.Roles))
            {
                bool hasRequiredRoles = attribute.Mode == RequireRolesAttribute.RoleCheckMode.All
                    ? _userContext.HasAllRoles(attribute.Roles.Split(',').Select(r => r.Trim()).ToArray())
                    : _userContext.HasAnyRole(attribute.Roles.Split(',').Select(r => r.Trim()).ToArray());

                if (!hasRequiredRoles)
                {
                    _logger.LogWarning("用户 {UserName} ({UserId}) 没有所需角色 {Roles} (模式: {Mode}) 访问 {Path}",
                        _userContext.UserName, _userContext.UserId, attribute.Roles, attribute.Mode, context.Request.Path);
                    return Task.FromResult(false);
                }
            }
        }

        if (rolesToCheck.Count > 0 && !_userContext.HasAnyRole(rolesToCheck.ToArray()))
        {
            _logger.LogWarning("用户 {UserName} ({UserId}) 没有足够的角色 {Roles} 访问 {Path}",
                _userContext.UserName, _userContext.UserId, string.Join(", ", rolesToCheck), context.Request.Path);
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
}
