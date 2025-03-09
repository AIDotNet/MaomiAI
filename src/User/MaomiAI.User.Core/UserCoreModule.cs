// <copyright file="UserModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.User.Core;

/// <summary>
/// 用户模块.
/// </summary>
public class UserCoreModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        // 注册中介者
        context.Services.AddScoped<IMediator, Mediator>();
        
        // 使用扫描程序集的方式自动注册处理程序
        RegisterHandlers(context.Services, typeof(UserCoreModule).Assembly);
        
        // 配置JWT认证
        ConfigureAuthentication(context);
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly assembly)
    {
        // 注册所有的请求处理程序
        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface && t.Name.EndsWith("Handler"))
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && 
                                              (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) || 
                                               i.GetGenericTypeDefinition() == typeof(IRequestHandler<>))));

        foreach (var handlerType in handlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && 
                           (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) || 
                            i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)));

            services.AddScoped(interfaceType, handlerType);
        }
    }

    private static void ConfigureAuthentication(ServiceContext context)
    {
        // 配置JWT认证
        var configuration = context.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        var jwtSettings = configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"] ?? "MaomiAIDefaultSecretKey1234567890ABCDEFGH";
        
        context.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"] ?? "MaomiAI",
                ValidAudience = jwtSettings["Audience"] ?? "MaomiAPIClient",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                }
            };
        });

        context.Services.AddAuthorization();
    }
} 