﻿// <copyright file="AuditsInfo.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra.Models;

/// <summary>
/// 数据子项.
/// </summary>
public class AuditsInfo
{
    /// <summary>
    /// 创建时间.
    /// </summary>
    public DateTimeOffset CreateTime { get; set; }

    /// <summary>
    /// 创建人ID.
    /// </summary>
    public Guid CreateUserId { get; set; }

    /// <summary>
    /// 创建者 名字.
    /// </summary>
    public string CreateUserName { get; set; }

    /// <summary>
    /// 更新时间.
    /// </summary>
    public DateTimeOffset UpdateTime { get; set; }

    /// <summary>
    /// 更新人ID.
    /// </summary>
    public Guid UpdateUserId { get; set; }

    /// <summary>
    /// 更新人 名字.
    /// </summary>
    public string UpdateUserName { get; set; }
}
