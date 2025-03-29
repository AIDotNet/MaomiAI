// <copyright file="Program.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.UseMaomiAI();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Environment.ContentRootPath),
});

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();
app.UseMaomiAIMiddleware();

app.UseHttpLogging();

app.MapControllers();

app.Run();