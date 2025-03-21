// <copyright file="Program.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.UseMaomiAI();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(); // scalar/v1
    app.MapOpenApi(); // /openapi/v1.json
}

//app.UseStaticFiles(new StaticFileOptions
//{
//    RequestPath = "/files/",
//    FileProvider = new LocalPhysicalFileProvider(),
//});

app.UseAuthentication();
app.UseAuthorization();
 
app.UseRouting();
app.UseMaomiAIMiddleware();

app.UseHttpLogging();

app.MapControllers();

app.Run();