// <copyright file="IModificationAudited.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Database.Audits
{
    /// <summary>
    /// 修改审计属性.
    /// </summary>
    public interface IModificationAudited
    {
        /// <summary>
        /// 修改人.
        /// </summary>
        Guid UpdateUserId { get; set; }

        /// <summary>
        /// 修改时间.
        /// </summary>
        DateTimeOffset UpdateTime { get; set; }
    }
}