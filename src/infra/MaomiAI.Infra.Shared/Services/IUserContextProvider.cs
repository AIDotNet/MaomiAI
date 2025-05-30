﻿// <copyright file="IUserContextProvider.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;

namespace MaomiAI.Infra.Services;

/// <summary>
/// 用户上下文提供者.
/// </summary>
public interface IUserContextProvider
{
    /// <summary>
    /// 获取用户上下文.
    /// </summary>
    /// <returns><see cref="UserContext"/>.</returns>
    UserContext GetUserContext();
}
