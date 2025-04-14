using FluentValidation;
using FluentValidation.AspNetCore;
using Maomi;
using MaomiAI.Filters;
using MaomiAI.Infra;
using MaomiAI.Services;
using MaomiAI.User.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MaomiAI.Modules;

/// <summary>
/// 配置 MVC .
/// </summary>
public class ConfigureMVCModule : IModule
{
    private readonly IConfiguration _configuration;
    private readonly SystemOptions _systemOptions;

    public ConfigureMVCModule(IConfiguration configuration)
    {
        _configuration = configuration;
        _systemOptions = configuration.Get<SystemOptions>()!;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        context.Services.AddControllers(options =>
        {
            options.Filters.Add<MVCExceptionFilter>();
        })
        .ConfigureApplicationPartManager(apm =>
        {
            apm.ApplicationParts.Add(new AssemblyPart(typeof(UserApiModule).Assembly));
        });

        context.Services.AddFluentValidationAutoValidation();

        context.Services.AddValidatorsFromAssemblies(context.Modules.Select(x => x.Assembly));
    }
}

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

        // 配置JWT选项
        IConfigurationSection jwtSection = context.Configuration.GetSection(JwtOptions.SectionName);
        context.Services.Configure<JwtOptions>(jwtSection);

        // 注册JwtOptions为单例服务
        context.Services.AddSingleton(resolver =>
            resolver.GetRequiredService<IOptions<JwtOptions>>().Value);

        JwtOptions? jwtOptions = jwtSection.Get<JwtOptions>();

        if (jwtOptions == null)
        {
            throw new ArgumentException($"Configuration section '{JwtOptions.SectionName}' is missing or invalid.");
        }

        // 添加JWT服务
        context.Services.AddSingleton<IJwtService, JwtService>();

        // 配置JWT认证
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        context.Services.AddAuthorization();
    }
}