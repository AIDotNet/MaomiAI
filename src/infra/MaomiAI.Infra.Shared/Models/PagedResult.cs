// <copyright file="PagedResult.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra.Models;

/// <summary>
/// 分页参数.
/// </summary>
public class PagedParamter
{
    /// <summary>
    /// 页码，从1开始.
    /// </summary>
    public int PageNo { get; set; } = 1;

    /// <summary>
    /// 每页大小.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
