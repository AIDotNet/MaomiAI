// <copyright file="Program.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.UseMaomiAI();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(); // scalar/v1
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.UseHttpLogging();

app.MapControllers();

app.Run();
