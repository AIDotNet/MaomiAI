﻿// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Maomi;
using MaomiAI.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostgreScaffold;
using System.Diagnostics;
using System.Text;

public class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("当前工作目录: " + Directory.GetCurrentDirectory());

        Console.WriteLine("请在目录下执行 dotnet run，请勿直接启动该项目");
        Console.WriteLine("使用前先删除 Data、Entities 两个目录，用完后也要删除");

        var assemblyDirectory = Directory.GetParent(typeof(Program).Assembly.Location);
        if (assemblyDirectory.FullName.Contains("bin"))
        {
            assemblyDirectory = assemblyDirectory.Parent.Parent.Parent;
        }

        string projectDirectory = assemblyDirectory.FullName;
        Directory.SetCurrentDirectory(projectDirectory);
        Console.WriteLine("当前工作目录: " + projectDirectory);

        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton<IConfigurationManager>(builder.Configuration);
        builder.Services.AddLogging();
        builder.Services.AddModule<DBModule>();
        var ioc = builder.Services.BuildServiceProvider();
        var systemOptions = ioc.GetRequiredService<SystemOptions>();

        // 本机已经安装需先安装 dotnet-ef
        // dotnet tool install -g dotnet-ef
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            WorkingDirectory = projectDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        processStartInfo.ArgumentList.Add("ef");
        processStartInfo.ArgumentList.Add("dbcontext");
        processStartInfo.ArgumentList.Add("scaffold");
        processStartInfo.ArgumentList.Add($"\"{systemOptions.Database}\"");
        processStartInfo.ArgumentList.Add("Npgsql.EntityFrameworkCore.PostgreSQL");
        processStartInfo.ArgumentList.Add("--context-dir");
        processStartInfo.ArgumentList.Add("Data");
        processStartInfo.ArgumentList.Add("--output-dir");
        processStartInfo.ArgumentList.Add("Entities");
        processStartInfo.ArgumentList.Add("--namespace");
        processStartInfo.ArgumentList.Add("MaomiAI.Database.Entities");
        processStartInfo.ArgumentList.Add("--context-namespace");
        processStartInfo.ArgumentList.Add("MaomiAI.Database");
        processStartInfo.ArgumentList.Add("--no-onconfiguring");
        processStartInfo.ArgumentList.Add("-f");

        processStartInfo.Arguments = string.Join(" ", processStartInfo.ArgumentList);
        processStartInfo.ArgumentList.Clear();
        var command = $"{processStartInfo.FileName} {processStartInfo.Arguments}";
        Console.WriteLine($"启动命令: {command}");

        using (var process = new Process { StartInfo = processStartInfo })
        {
            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine(e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Data);
                    Console.ResetColor();
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();
        }
    }
}