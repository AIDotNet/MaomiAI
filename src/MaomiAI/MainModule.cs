// <copyright file="MainModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using MaomiAI.Database;
using MaomiAI.Infra;
using MaomiAI.Store;
using MaomiAI.Team.Core;
using MaomiAI.User.Api;
using MaomiAI.User.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;

namespace MaomiAI
{
    /// <summary>
    /// MainModule.
    /// </summary>
    [InjectModule<InfraCoreModule>]
    [InjectModule<DatabaseCoreModule>]
    [InjectModule<EmbeddingCoreModule>]
    [InjectModule<DocumentModule>]
    [InjectModule<StoreCoreModule>]
    [InjectModule<UserCoreModule>]
    [InjectModule<TeamCoreModule>]
    public partial class MainModule : IModule
    {
        private readonly IConfiguration _configuration;
        private readonly SystemOptions _systemOptions;

        public MainModule(IConfiguration configuration)
        {
            _configuration = configuration;
            _systemOptions = configuration.Get<SystemOptions>()!;
        }

        /// <inheritdoc/>
        public void ConfigureServices(ServiceContext context)
        {
            ConfigureHttpLogging(context);

            context.Services.AddControllers(options => { })
                .ConfigureApplicationPartManager(apm =>
                {
                    apm.ApplicationParts.Add(new AssemblyPart(typeof(UserApiModule).Assembly));
                });

            ConfigureOpeiApi(context);

            context.Services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(context.Modules.Select(x => x.Assembly).ToArray());
            });
        }

        // 配置 http 请求日志.
        private void ConfigureHttpLogging(ServiceContext context)
        {
            // todo: 后续是否允许在配置文件指定 LoggingFields 参数
            context.Services.AddHttpLogging(logging =>
            {
                //logging.LoggingFields = HttpLoggingFields.All;
                logging.CombineLogs = true;
            });
        }

        private void ConfigureOpeiApi(ServiceContext context)
        {
            context.Services.AddOpenApi(options =>
            {
                options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Title = "Maomi API",
                        Version = "v1",
                        Description = "MaomiAI openapi document."
                    };

                    document.Servers = new List<OpenApiServer>
                    {
                        new()
                        {
                            Url = _systemOptions.Server,
                            Description = "User-defined service address"
                        }
                    };

                    return Task.CompletedTask;
                });

                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                options.AddSchemaTransformer<TypeSchemeTransformer>();
            });
        }
    }
}