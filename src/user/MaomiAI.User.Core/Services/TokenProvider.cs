using Maomi;
using Maomi.AI.Exceptions;
using Maomi.I18n;
using MaomiAI.Infra;
using MaomiAI.Infra.Models;
using MaomiAI.Infra.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MaomiAI.User.Core.Services;

/// <summary>
/// 构建 token.
/// </summary>
[InjectOnScoped]
public class TokenProvider : ITokenProvider
{
    private readonly IRsaProvider _rsaProvider;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenProvider"/> class.
    /// </summary>
    /// <param name="rsaProvider"></param>
    /// <param name="systemOptions"></param>
    public TokenProvider(IRsaProvider rsaProvider, SystemOptions systemOptions)
    {
        _rsaProvider = rsaProvider;
        _systemOptions = systemOptions;
    }

    /// <inheritdoc/>
    public (string AccessToken, string RefreshToken) GenerateTokens(UserContext userContext)
    {
        Claim[] claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, userContext.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, userContext.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, userContext.UserName),
                new Claim(JwtRegisteredClaimNames.Nickname, userContext.NickName),
                new Claim(JwtRegisteredClaimNames.Email, userContext.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("token_type", "access")
        };

        var rsaKey = new RsaSecurityKey(_rsaProvider.GetPrivateRsa());

        // 生成 Access Token
        var accessTokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = claims.ToDictionary(x => x.Type, x => (object)x.Value),
            Subject = new ClaimsIdentity(claims),
            Issuer = _systemOptions.Server,
            Audience = _systemOptions.Server,
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256),
            TokenType = "AccessToken",
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);

        // 生成 Refresh Token
        var resfrshTokenClaims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userContext.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.NameId, userContext.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("token_type", "refresh")
            };
        var refreshTokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = resfrshTokenClaims.ToDictionary(x => x.Type, x => (object)x.Value),
            Subject = new ClaimsIdentity(resfrshTokenClaims),
            Issuer = _systemOptions.Server,
            Audience = _systemOptions.Server,
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256)
        };

        var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);

        return (tokenHandler.WriteToken(accessToken), tokenHandler.WriteToken(refreshToken));
    }

    /// <inheritdoc/>
    public async Task<TokenValidationResult> ValidateTokenAsync(string token)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        if (!jwtSecurityTokenHandler.CanReadToken(token))
        {
            throw new BusinessException("不是有效的 token 格式.");
        }

        var rsaKey = new RsaSecurityKey(_rsaProvider.GetPrivateRsa());

        var checkResult = await jwtSecurityTokenHandler.ValidateTokenAsync(token, new TokenValidationParameters()
        {
            RequireExpirationTime = true,
            ValidateIssuer = true,

            ValidIssuer = _systemOptions.Server,
            ValidAudience = _systemOptions.Server,

            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = rsaKey,
        });

        return checkResult;
    }

    /// <inheritdoc/>
    public async Task<(UserContext UserContext, IReadOnlyDictionary<string, Claim> Claims)> ParseUserContextFromTokenAsync(string token)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        if (!jwtSecurityTokenHandler.CanReadToken(token))
        {
            throw new BusinessException("不是有效的 token 格式.");
        }

        var rsaKey = new RsaSecurityKey(_rsaProvider.GetPrivateRsa());

        var checkResult = await jwtSecurityTokenHandler.ValidateTokenAsync(token, new TokenValidationParameters()
        {
            RequireExpirationTime = true,
            ValidateIssuer = true,

            ValidIssuer = _systemOptions.Server,
            ValidAudience = _systemOptions.Server,

            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = rsaKey,
        });

        if (!checkResult.IsValid)
        {
            throw new BusinessException("token 验证失败.");
        }

        var jwt = jwtSecurityTokenHandler.ReadJwtToken(token);
        var claims = jwt.Claims.ToArray();

        // 从 Claims 中解析 UserContext
        var userContext = new DefaultUserContext
        {
            UserId = Guid.Parse(claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? Guid.Empty.ToString()),
            UserName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value!,
            NickName = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Nickname)?.Value!,
            Email = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value!,
            IsAuthenticated = true
        };

        return (userContext, claims.ToDictionary(x => x.Type, x => x));
    }
}
