// <copyright file="AppConsts.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra;

/// <summary>
/// 系统常用信息.
/// </summary>
public static class AppConsts
{
    /// <summary>
    /// 程序路径.
    /// </summary>
    public static string AppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase!;
    public static string ConfigsPath = Path.Combine(AppPath, "configs");
    public static string DefaultConfigsPath = Path.Combine(AppPath, "default_configs");
    public static string LoggerJson = Path.Combine(ConfigsPath, "logger.json");
    public static string DefaultLoggerJson = Path.Combine(DefaultConfigsPath, "logger.json");
    public static string RSA = Path.Combine(ConfigsPath, "rsa_private.key");
}
