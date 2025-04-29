// <copyright file="PublicController.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Controllers.Public;

/// <summary>
/// 公共选项.
/// </summary>
public class PublicOptions
{
    /// <summary>
    /// 系统访问地址.
    /// </summary>
    public string ServiceUrl { get; init; }

    /// <summary>
    /// 公共存储地址，静态资源时可直接访问.
    /// </summary>
    public string PublicStoreUrl { get; init; }
}