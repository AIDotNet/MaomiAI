using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities
{
    /// <summary>
    /// 团队.
    /// </summary>
    public partial class TeamEntity : IFullAudited
    {
        /// <summary>
        /// 私有构造函数，防止直接实例化
        /// </summary>
        private TeamEntity()
        {
            Uuid = Guid.NewGuid();
            CreateTime = DateTimeOffset.UtcNow;
            UpdateTime = DateTimeOffset.UtcNow;
            Status = true;
            IsDeleted = false;
        }

        /// <summary>
        /// 创建新团队
        /// </summary>
        /// <param name="name">团队名称</param>
        /// <param name="description">团队描述</param>
        /// <param name="avatar">团队头像URL</param>
        /// <param name="createUserId">创建人ID</param>
        /// <returns>新的团队实体</returns>
        public static TeamEntity Create(
            string name,
            string description,
            string? avatar = null,
            Guid? createUserId = null)
        {
            TeamEntity? team = new()
            {
                Name = name,
                Description = description,
                Avatar = avatar ?? string.Empty,
                CreateUserId = createUserId ?? Guid.Empty,
                UpdateUserId = createUserId ?? Guid.Empty
            };

            return team;
        }

        /// <summary>
        /// 更新团队信息
        /// </summary>
        /// <param name="name">团队名称</param>
        /// <param name="description">团队描述</param>
        /// <param name="avatar">团队头像URL</param>
        /// <param name="updateUserId">更新人ID</param>
        public void Update(string? name = null, string? description = null, string? avatar = null,
            Guid? updateUserId = null)
        {
            if (name != null)
            {
                Name = name;
            }

            if (description != null)
            {
                Description = description;
            }

            if (avatar != null)
            {
                Avatar = avatar;
            }

            if (updateUserId.HasValue)
            {
                UpdateUserId = updateUserId.Value;
            }

            UpdateTime = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// 更改团队状态
        /// </summary>
        /// <param name="status">新状态</param>
        /// <param name="updateUserId">更新人ID</param>
        public void ChangeStatus(bool status, Guid? updateUserId = null)
        {
            Status = status;

            if (updateUserId.HasValue)
            {
                UpdateUserId = updateUserId.Value;
            }

            UpdateTime = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// 标记为删除
        /// </summary>
        /// <param name="updateUserId">更新人ID</param>
        public void MarkAsDeleted(Guid? updateUserId = null)
        {
            IsDeleted = true;

            if (updateUserId.HasValue)
            {
                UpdateUserId = updateUserId.Value;
            }

            UpdateTime = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// 团队描述.
        /// </summary>
        public string Description { get; private set; } = null!;

        /// <summary>
        /// 团队名称.
        /// </summary>
        public string Name { get; private set; } = null!;

        /// <summary>
        /// 团队头像.
        /// </summary>
        public string Avatar { get; private set; } = null!;

        /// <summary>
        /// uuid.
        /// </summary>
        public Guid Uuid { get; private set; }

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

        /// <summary>
        /// 状态：true-正常，false-禁用.
        /// </summary>
        public bool Status { get; private set; }
    }
}