using Maomi;
using MaomiAI.Middlewares;
using MaomiAI.Services;
using MaomiAI.User.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace MaomiAI
{
    public static class MainExtensions
    {
        public static IHostApplicationBuilder UseMaomiAI(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IConfigurationManager>(builder.Configuration);

            // 注册系统配置
            builder.Services.Configure<SystemOptions>(builder.Configuration);
            builder.Services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<SystemOptions>>().Value);

            builder.Services.AddSerilog((services, configuration) =>
            {
                configuration.ReadFrom.Services(services);
                configuration.ReadFrom.Configuration(builder.Configuration);
            });

            // 添加HTTP上下文访问器
            builder.Services.AddHttpContextAccessor();

            // 配置JWT选项
            IConfigurationSection jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
            builder.Services.Configure<JwtOptions>(jwtSection);

            // 注册JwtOptions为单例服务
            builder.Services.AddSingleton(resolver =>
                resolver.GetRequiredService<IOptions<JwtOptions>>().Value);

            JwtOptions? jwtOptions = jwtSection.Get<JwtOptions>();

            if (jwtOptions != null)
            {
                // 添加JWT服务
                builder.Services.AddSingleton<IJwtService, JwtService>();

                // 配置JWT认证
                builder.Services.AddAuthentication(options =>
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
            }

            builder.Services.AddModule<MainModule>();

            return builder;
        }

        public static WebApplication UseMaomiAIMiddleware(this WebApplication app)
        {
            // 使用JWT中间件
            app.UseMiddleware<JwtMiddleware>();

            // 使用认证中间件
            app.UseMiddleware<AuthMiddleware>();

            return app;
        }
    }
}