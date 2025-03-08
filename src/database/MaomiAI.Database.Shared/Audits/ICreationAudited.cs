// <copyright file="ICreationAudited.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Database.Audits;

/// <summary>
/// 创建审计属性.
/// </summary>
public interface ICreationAudited
{
    /// <summary>
    /// 创建人的用户名.
    /// </summary>
    int CreatorUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    DateTimeOffset CreationTime { get; set; }
}
