﻿// <copyright file="FastEndpointModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FastEndpoints.Swagger;
using Maomi;
using MaomiAI.Infra;
using MaomiAI.Infra.JsonConverters;
using MaomiAI.Swaggers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;
using NSwag;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace MaomiAI.Modules;

/// <summary>
/// 使用 FastEndpoints 作为 API 的模块.
/// </summary>
public class FastEndpointModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        context.Services
            .AddFastEndpoints()
            .SwaggerDocument(options =>
            {
                var settings = options.Services.GetRequiredService<SystemOptions>();
                var serverAddressesFeature = options.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>();

                options.DocumentSettings = s =>
                {
                    s.Title = "AI API";
                    s.Version = "v1";
                    s.Description = "MaomiAI openapi document.";
                    s.AddAuth("Bearer", new()
                    {
                        Type = OpenApiSecuritySchemeType.Http,
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        BearerFormat = "JWT",
                    });

                    s.MarkNonNullablePropsAsRequired();
                    s.PostProcess = d =>
                    {
                        d.Servers.Add(new OpenApiServer
                        {
                            Url = settings.Server,
                            Description = "User-defined service address"
                        });

                        if (serverAddressesFeature != null)
                        {
                            foreach (var address in serverAddressesFeature.Addresses)
                            {
                                d.Servers.Add(new OpenApiServer
                                {
                                    Url = address,
                                    Description = "Local service address"
                                });
                            }
                        }
                    };

                    // 要与 SerializerSettings 对应序列化配置和显示的 Swagger 类型
                    // todo: 后续添加，https://maomi.whuanle.cn/10.web.html#%E6%A8%A1%E5%9E%8B%E7%B1%BB%E5%B1%9E%E6%80%A7%E7%B1%BB%E5%9E%8B%E5%A4%84%E7%90%86
                    s.SchemaSettings.TypeMappers.Add(
                        new PrimitiveTypeMapper(
                            typeof(Guid),
                            schema =>
                                {
                                    schema.Type = JsonObjectType.String;
                                    schema.Format = "uuid";
                                }));
                    s.SchemaSettings.TypeMappers.Add(new LongTypeMapper());
                    s.SchemaSettings.TypeMappers.Add(new DateTimeOffsetTypeMapper());

                    s.OperationProcessors.Add(new ErrorResponseOperationProcessor());
                };

                options.SerializerSettings = s =>
                {
                    s.PropertyNamingPolicy = null;
                    s.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

                    // todo: 后续添加，https://maomi.whuanle.cn/10.web.html#%E6%A8%A1%E5%9E%8B%E7%B1%BB%E5%B1%9E%E6%80%A7%E7%B1%BB%E5%9E%8B%E5%A4%84%E7%90%86
                    s.Converters.Add(new JsonStringEnumConverter());
                    s.Converters.Add(new DateTimeOffsetConverter());
                    s.Converters.Add(JsonMetadataServices.DateTimeConverter);
                    s.Converters.Add(JsonMetadataServices.DateOnlyConverter);
                    s.Converters.Add(JsonMetadataServices.DecimalConverter);
                };
            });
    }
}
