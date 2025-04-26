// <copyright file="IClientInfoProvider.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;

namespace MaomiAI.Infra.Services;

public interface IClientInfoProvider
{
    ClientInfo GetClientInfo();
}