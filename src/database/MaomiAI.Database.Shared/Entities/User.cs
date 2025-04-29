using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 用户表.
/// </summary>
public partial class UserEntity : IFullAudited
{
    /// <summary>
    /// 用户ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 用户名.
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 邮箱.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// 密码.
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// 昵称.
    /// </summary>
    public string NickName { get; set; } = null!;

    /// <summary>
    /// 头像路径.
    /// </summary>
    public string AvatarPath { get; set; } = null!;

    /// <summary>
    /// 手机号.
    /// </summary>
    public string Phone { get; set; } = null!;

    /// <summary>
    /// 是否启用.
    /// </summary>
    public bool IsEnable { get; set; }

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
    /// 计算密码值的salt.
    /// </summary>
    public string PasswordHalt { get; set; } = null!;

    /// <summary>
    /// 头像id.
    /// </summary>
    public Guid AvatarId { get; set; }
}
