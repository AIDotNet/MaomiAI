// <copyright file="IRsaProvider.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.Security.Cryptography;

namespace MaomiAI.Infra.Service
{
    public interface IRsaProvider
    {
        string Decrypt(string message, RSAEncryptionPadding? padding = null);
        string Encrypt(string message, RSAEncryptionPadding? padding = null);
        string ExportPublichKeyPck8();
    }
}