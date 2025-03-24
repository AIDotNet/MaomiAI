﻿using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities
{
    /// <summary>
    /// 系统配置.
    /// </summary>
    public partial class SettingEntity : IFullAudited
    {
        /// <summary>
        /// 配置项值.
        /// </summary>
        public string Value { get; set; } = null!;

        /// <summary>
        /// id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 配置名称.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 创建者id.
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// 是否删除.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }

        /// <summary>
        /// 更新时间.
        /// </summary>
        public DateTimeOffset UpdateTime { get; set; }

        /// <summary>
        /// 创建人ID.
        /// </summary>
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 更新人ID.
        /// </summary>
        public Guid UpdateUserId { get; set; }
    }
}