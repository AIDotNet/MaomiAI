// <copyright file="HashHelper.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.Security.Cryptography;
using System.Text;

namespace MaomiAI.User.Core.Services;

/// <summary>
/// 哈希助手类.
/// </summary>
public static class HashHelper
{
    /// <summary>
    /// 计算SHA256哈希值.
    /// </summary>
    /// <param name="text">要哈希的文本.</param>
    /// <returns>哈希值的十六进制字符串表示.</returns>
    public static string ComputeSha256Hash(string text)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(text);
        var hash = sha256.ComputeHash(bytes);
        
        var builder = new StringBuilder();
        foreach (var b in hash)
        {
            builder.Append(b.ToString("x2"));
        }
        
        return builder.ToString();
    }
} 