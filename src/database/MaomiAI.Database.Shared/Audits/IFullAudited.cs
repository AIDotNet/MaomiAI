// <copyright file="IFullAudited.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>


namespace MaomiAI.Database.Audits
{
    /// <summary>
    /// 全部审计属性.
    /// </summary>
    public interface IFullAudited : ICreationAudited, IModificationAudited, IDeleteAudited
    {
    }

    /// <summary>
    /// 全部审计属性.
    /// </summary>
    public class FullAudited : IFullAudited
    {
        /// <inheritdoc/>
        public virtual bool IsDeleted { get; set; }

        /// <inheritdoc/>
        public virtual DateTimeOffset CreateTime { get; set; }

        /// <inheritdoc/>
        public virtual DateTimeOffset UpdateTime { get; set; }

        /// <inheritdoc/>
        public virtual Guid CreateUserId { get; set; }

        /// <inheritdoc/>
        public virtual Guid UpdateUserId { get; set; }
    }
}