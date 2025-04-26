using Maomi;
using MaomiAI.Infra;
using MaomiAI.Infra.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace MaomiAI.Modules;

/// <summary>
/// 配置 授权 .
/// </summary>
public class ConfigureAuthorizaModule : IModule
{
    private readonly IConfiguration _configuration;
    private readonly SystemOptions _systemOptions;

    public ConfigureAuthorizaModule(IConfiguration configuration)
    {
        _configuration = configuration;
        _systemOptions = configuration.Get<SystemOptions>()!;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        var rsaKey = new RsaSecurityKey(RSA.Create());

        context.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = rsaKey,
                    ValidateIssuer = true,
                    ValidIssuer = _systemOptions.Server,
                    ValidateAudience = true,
                    ValidAudience = _systemOptions.Server,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        context.Services.AddAuthorization();
    }
}