// <copyright file="Program.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.UseMaomiAI();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "/openapi/{documentName}.json");
    app.MapScalarApiReference();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Environment.ContentRootPath),
});

app.UseDefaultExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.UseMaomiAIMiddleware();

app.UseHttpLogging();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    c.Errors.ProducesMetadataType = typeof(ErrorResponse);
    c.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
    {
        return new MaomiAI.Infra.Models.ErrorResponse(failures, statusCode)
        {
            Message = "请求参数验证失败",
            RequestId = ctx.TraceIdentifier,
        };
    };
});

app.Run();