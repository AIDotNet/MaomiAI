// <copyright file="RsaProvider.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Helpers;
using MaomiAI.Infra.Services;
using System.Security.Cryptography;

namespace MaomiAI.Infra.Service
{
    public class RsaProvider : IRsaProvider, IDisposable
    {
        private readonly RSA _rsaPrivate;

        public RsaProvider(string rsaPem)
        {
            _rsaPrivate = RSA.Create();
            _rsaPrivate.ImportFromPem(rsaPem);
        }

        public string ExportPublichKeyPck8()
        {
            return RsaHelper.ExportPublichKeyPck8(_rsaPrivate);
        }

        public string Encrypt(string message, RSAEncryptionPadding? padding = null)
        {
            if (padding == null)
            {
                padding = RSAEncryptionPadding.OaepSHA256;
            }

            return RsaHelper.Encrypt(_rsaPrivate, message, padding);
        }

        public string Decrypt(string message, RSAEncryptionPadding? padding = null)
        {
            if (padding == null)
            {
                padding = RSAEncryptionPadding.OaepSHA256;
            }

            return RsaHelper.Decrypt(_rsaPrivate, message, padding);
        }

        public void Dispose()
        {
            ((IDisposable)_rsaPrivate).Dispose();
        }
    }
}