// <copyright file="Program.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI;
using MaomiAI.Store.Services;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.UseMaomiAI();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference().AllowAnonymous(); // scalar/v1
    app.MapOpenApi().AllowAnonymous();            // /openapi/v1.json
}

//app.UseStaticFiles(new StaticFileOptions
//{
//    RequestPath = "/files/",
//    FileProvider = new LocalPhysicalFileProvider(),
//});

// 使用JWT中间件
app.UseMaomiAIMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.UseHttpLogging();

app.MapControllers();

app.Run();
