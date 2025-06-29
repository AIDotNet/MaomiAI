﻿// <copyright file="EmptyCommandResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

#pragma warning disable CA1052 // 静态容器类型应为 Static 或 NotInheritable

namespace MaomiAI.Infra.Models;

/// <summary>
/// 空数据.
/// </summary>
public class EmptyCommandResponse
{
    /// <summary>
    /// 默认实例.
    /// </summary>
    public static readonly EmptyCommandResponse Default = new EmptyCommandResponse();

    private EmptyCommandResponse()
    {
    }
}
