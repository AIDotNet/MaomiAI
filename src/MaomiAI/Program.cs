// <copyright file="Program.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using MaomiAI;
using Serilog;
uint Maomi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IConfigurationManager>(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Host.UseSerilog();
builder.Services.AddModule<MainModule>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseHttpLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
