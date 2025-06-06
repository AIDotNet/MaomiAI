// <copyright file="Program.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;
using System;
using System.Linq;
using System.Text.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.UseMaomiAI();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c =>
    {
        c.Path = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.UseCors("AllowSpecificOrigins");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Environment.ContentRootPath),
});

app.UseDefaultExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.UseMaomiAIMiddleware();

app.UseHttpLogging();

app.UseFastEndpoints((Action<Config>?)(c =>
{
    c.Endpoints.ShortNames = true;

    c.Endpoints.RoutePrefix = "api";
    c.Errors.ProducesMetadataType = typeof(MaomiAI.Infra.Models.BusinessExceptionResponse);

    // 拦截一些验证异常等
    c.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
    {
        return (object)new MaomiAI.Infra.Models.BusinessExceptionResponse(failures, statusCode)
        {
            Detail = failures.FirstOrDefault()?.ErrorMessage ?? "请求参数错误",
            RequestId = ctx.TraceIdentifier,
        };
    };
    c.Serializer.Options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    c.Serializer.Options.Converters.Add(new System.Text.Json.LongStringConverter());
}));

app.Run();