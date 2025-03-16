// <copyright file="AESProvider.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.Security.Cryptography;
using System.Text;

namespace MaomiAI.Infra.Service;

public class AESProvider : IAESProvider
{
    private readonly string _key;
    private readonly byte[] _keyBytes;

    public AESProvider(string key)
    {
        _key = key;
        _keyBytes = Encoding.UTF8.GetBytes(_key);
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            throw new ArgumentNullException(nameof(plainText));
        }

        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = _keyBytes;
            aesAlg.GenerateIV();
            var iv = aesAlg.IV;

            var encryptor = aesAlg.CreateEncryptor();

            using (var msEncrypt = new MemoryStream())
            {
                // 先写入 IV
                msEncrypt.Write(iv, 0, iv.Length);

                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
        {
            throw new ArgumentNullException(nameof(cipherText));
        }

        byte[] fullCipherBytes = Convert.FromBase64String(cipherText);

        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = _keyBytes;

            // 从密文中提取 IV（前16字节）
            byte[] iv = new byte[16];
            byte[] actualCipherText = new byte[fullCipherBytes.Length - 16];

            Buffer.BlockCopy(fullCipherBytes, 0, iv, 0, 16);
            Buffer.BlockCopy(fullCipherBytes, 16, actualCipherText, 0, actualCipherText.Length);

            aesAlg.IV = iv;

            var decryptor = aesAlg.CreateDecryptor();

            using (var msDecrypt = new MemoryStream(actualCipherText))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
}