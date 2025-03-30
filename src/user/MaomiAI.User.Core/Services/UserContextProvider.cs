using Maomi;
using MaomiAI.Infra.Models;
using MaomiAI.User.Shared.Services;
using Microsoft.AspNetCore.Http;

namespace MaomiAI.User.Core.Services;

/// <summary>
/// 用户上下文提供者.
/// </summary>
[InjectOnScoped]
public class UserContextProvider : IUserContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserContextProvider"/> class.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public UserContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        _userContext = new Lazy<UserContext>(() =>
        {
            return Parse();
        });
    }

    private readonly Lazy<UserContext> _userContext;

    /// <inheritdoc/>
    public UserContext GetUserContext() => _userContext.Value;

    private DefaultUserContext Parse()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        ArgumentNullException.ThrowIfNull(httpContext);

        var user = httpContext.User;
        if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return new DefaultUserContext
            {
                IsAuthenticated = false,
                UserId = Guid.Empty,
                UserName = "Anonymous",
                NickName = "Anonymous",
                Email = string.Empty
            };
        }

        var userId = user.FindFirst("sub")?.Value;
        var userName = user.Identity.Name;
        var nickName = user.FindFirst("nickname")?.Value;
        var email = user.FindFirst("email")?.Value;

        return new DefaultUserContext
        {
            IsAuthenticated = true,
            UserId = Guid.TryParse(userId, out var guid) ? guid : Guid.Empty,
            UserName = userName ?? string.Empty,
            NickName = nickName ?? string.Empty,
            Email = email ?? string.Empty
        };
    }
}
