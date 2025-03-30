// <copyright file="PasswordService.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.Security.Cryptography;
using System.Text;

namespace MaomiAI.Infra.Helpers;

/// <summary>
/// 密码服务 - 提供密码哈希和验证功能
/// </summary>
public static class UserPasswordHelper
{
    private const int PBKDF2_ITERATIONS = 10000; // 迭代次数
    private const int SALT_SIZE = 16; // 盐值大小（字节）
    private const int HASH_SIZE = 32; // 哈希大小（字节）

    /// <summary>
    /// 使用PBKDF2算法哈希密码
    /// </summary>
    /// <param name="password">原始密码</param>
    /// <returns>格式化的哈希结果</returns>
    public static string HashPassword(string password)
    {
        // 生成随机盐值
        byte[] salt = new byte[SALT_SIZE];
        using (RandomNumberGenerator? rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // 使用PBKDF2派生密钥
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            PBKDF2_ITERATIONS,
            HashAlgorithmName.SHA256,
            HASH_SIZE);

        // 组合盐值和哈希结果
        byte[] hashBytes = new byte[SALT_SIZE + HASH_SIZE];
        Array.Copy(salt, 0, hashBytes, 0, SALT_SIZE);
        Array.Copy(hash, 0, hashBytes, SALT_SIZE, HASH_SIZE);

        // 返回Base64编码的结果
        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// 验证密码 - 支持新旧两种哈希格式
    /// </summary>
    /// <param name="password">原始密码</param>
    /// <param name="hashedPassword">哈希后的密码</param>
    /// <returns>密码是否匹配</returns>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // Base64编码的PBKDF2哈希
        return VerifyPbkdf2Password(password, hashedPassword);
    }

    /// <summary>
    /// 验证PBKDF2哈希密码
    /// </summary>
    /// <param name="password">要验证的密码</param>
    /// <param name="hashedPassword">存储的哈希密码</param>
    /// <returns>密码是否匹配</returns>
    public static bool VerifyPbkdf2Password(string password, string hashedPassword)
    {
        try
        {
            // 将Base64字符串转换回字节数组
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // 提取盐值（前SALT_SIZE字节）
            byte[] salt = new byte[SALT_SIZE];
            Array.Copy(hashBytes, 0, salt, 0, SALT_SIZE);

            // 提取存储的哈希值
            byte[] storedHash = new byte[HASH_SIZE];
            Array.Copy(hashBytes, SALT_SIZE, storedHash, 0, HASH_SIZE);

            // 使用相同的参数重新计算哈希值
            byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                PBKDF2_ITERATIONS,
                HashAlgorithmName.SHA256,
                HASH_SIZE);

            // 比较计算的哈希值与存储的哈希值
            return computedHash.SequenceEqual(storedHash);
        }
        catch
        {
            // 如果解析失败，返回验证失败
            return false;
        }
    }
}