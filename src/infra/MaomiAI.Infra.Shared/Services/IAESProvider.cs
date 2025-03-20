// <copyright file="IAESProvider.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra.Service
{
    public interface IAESProvider
    {
        string Decrypt(string cipherText);
        string Encrypt(string plainText);
    }
}