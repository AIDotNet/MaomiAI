using System.Security.Claims;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.User.Core.Services
{
    /// <summary>
    /// 认证中间件，负责从JWT令牌中提取用户信息并填充到UserContext中.
    /// </summary>
    public class AuthMiddleware : IMiddleware
    {
        private readonly DefaultUserContext _userContext;
        private readonly ILogger<AuthMiddleware> _logger;

        public AuthMiddleware(DefaultUserContext userContext, ILogger<AuthMiddleware> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _userContext.Clear(); // 清除用户上下文，避免请求间数据污染
            string? path = context.Request.Path.Value?.ToLower();

            if (IsAnonymousAllowed(context, path))
            {
                await next(context);
                return;
            }

            Console.WriteLine(context.User.Identity);

            if (context.User.Identity?.IsAuthenticated != true)
            {
                _logger.LogWarning("未认证的请求尝试访问 {Path}", path);
                await SetResponseAsync(context, StatusCodes.Status401Unauthorized, "未认证的请求");
                return;
            }

            AuthenticationResult authResult = await AuthenticateUserAsync(context);
            if (!authResult.IsAuthenticated)
            {
                _logger.LogWarning("认证失败: {Reason}", authResult.ErrorMessage);
                await SetResponseAsync(context, StatusCodes.Status403Forbidden, authResult.ErrorMessage);
                return;
            }

            if (!await CheckAuthorizationRequirements(context))
            {
                _logger.LogWarning("用户 {UserName} ({UserId}) 没有足够的权限访问 {Path}", _userContext.UserName,
                    _userContext.UserId, path);
                await SetResponseAsync(context, StatusCodes.Status403Forbidden, "没有足够的权限访问此资源");
                return;
            }

            await next(context);
        }

        // 改进后的方法，确保匿名路径能正确识别
        private bool IsAnonymousAllowed(HttpContext context, string? path)
        {
            if (!string.IsNullOrEmpty(path) && (path.StartsWith("/scalar") || path.StartsWith("/openapi")))
            {
                _logger.LogDebug("允许 API 测试工具路径 {Path} 匿名访问", path);
                return true;
            }

            Endpoint? endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                _logger.LogDebug("请求路径 {Path} 允许匿名访问", path);
                return true;
            }

            return false;
        }

        private async Task<AuthenticationResult> AuthenticateUserAsync(HttpContext context)
        {
            Claim? userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            Claim? userNameClaim = context.User.FindFirst(ClaimTypes.Name);
            Claim? nickNameClaim = context.User.FindFirst("nickname");
            Claim? emailClaim = context.User.FindFirst(ClaimTypes.Email);
            Claim? avatarClaim = context.User.FindFirst("avatar");

            if (userIdClaim is null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return AuthenticationResult.Failed("用户ID解析失败");
            }

            _userContext
                .SetUserInfo(
                    userId,
                    userNameClaim?.Value ?? string.Empty,
                    nickNameClaim?.Value ?? string.Empty,
                    emailClaim?.Value ?? string.Empty,
                    avatarClaim?.Value ?? string.Empty)
                .SetAuthenticated(true)
                .AddRoles(context.User.FindAll(ClaimTypes.Role).Select(c => c.Value));

            _logger.LogInformation("用户 {UserName} ({UserId}) 已认证", userNameClaim?.Value, userId);
            return AuthenticationResult.Success();
        }

        private async Task<bool> CheckAuthorizationRequirements(HttpContext context)
        {
            Endpoint? endpoint = context.GetEndpoint();
            if (endpoint is null)
            {
                return true;
            }

            HashSet<string> requiredRoles = new();

            foreach (AuthorizeAttribute attribute in endpoint.Metadata.GetOrderedMetadata<AuthorizeAttribute>())
            {
                if (!string.IsNullOrEmpty(attribute.Roles))
                {
                    requiredRoles.UnionWith(attribute.Roles.Split(',').Select(r => r.Trim()));
                }
            }

            if (requiredRoles.Count > 0 && !_userContext.HasAnyRole(requiredRoles.ToArray()))
            {
                _logger.LogWarning("用户 {UserName} ({UserId}) 没有足够的角色 {Roles} 访问 {Path}",
                    _userContext.UserName, _userContext.UserId, string.Join(", ", requiredRoles), context.Request.Path);
                return false;
            }

            return true;
        }

        private async Task SetResponseAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new { message });
        }

        private record AuthenticationResult(bool IsAuthenticated, string? ErrorMessage)
        {
            public static AuthenticationResult Success()
            {
                return new AuthenticationResult(true, null);
            }

            public static AuthenticationResult Failed(string message)
            {
                return new AuthenticationResult(false, message);
            }
        }
    }
}