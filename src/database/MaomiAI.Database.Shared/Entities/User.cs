using System;
using System.Collections.Generic;

using MaomiAI.Database.Audits;

namespace MaomiAI.Database.Entities;

/// <summary>
/// 用户表.
/// </summary>
public partial class UserEntity : IFullAudited
{
    /// <summary>
    /// 私有构造函数，防止直接实例化
    /// </summary>
    private UserEntity()
    {
        Id = Guid.NewGuid();
        CreateTime = DateTimeOffset.UtcNow;
        UpdateTime = DateTimeOffset.UtcNow;
        Status = true;
        IsDeleted = false;
        Extensions = "{}";
    }

    /// <summary>
    /// 创建新用户
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="email">邮箱</param>
    /// <param name="password">密码（已哈希）</param>
    /// <param name="nickName">昵称</param>
    /// <param name="avatarUrl">头像URL</param>
    /// <param name="phone">手机号</param>
    /// <returns>新的用户实体</returns>
    public static UserEntity Create(
        string userName,
        string email,
        string password,
        string nickName,
        string? avatarUrl = null,
        string? phone = null)
    {
        var user = new UserEntity
        {
            UserName = userName,
            Email = email,
            Password = password,
            NickName = nickName,
            AvatarUrl = avatarUrl ?? string.Empty,
            Phone = phone ?? string.Empty
        };

        return user;
    }

    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="email">邮箱</param>
    /// <param name="nickName">昵称</param>
    /// <param name="avatarUrl">头像URL</param>
    /// <param name="phone">手机号</param>
    public void Update(string? email = null, string? nickName = null, string? avatarUrl = null, string? phone = null)
    {
        if (email != null)
        {
            Email = email;
        }

        if (nickName != null)
        {
            NickName = nickName;
        }

        if (avatarUrl != null)
        {
            AvatarUrl = avatarUrl;
        }

        if (phone != null)
        {
            Phone = phone;
        }

        UpdateTime = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// 更改用户状态
    /// </summary>
    /// <param name="status">新状态</param>
    public void ChangeStatus(bool status)
    {
        Status = status;
        UpdateTime = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// 更改密码
    /// </summary>
    /// <param name="newPassword">新密码（已哈希）</param>
    public void ChangePassword(string newPassword)
    {
        Password = newPassword;
        UpdateTime = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// 标记为删除
    /// </summary>
    public void MarkAsDeleted()
    {
        IsDeleted = true;
        UpdateTime = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// 用户ID.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// 用户名.
    /// </summary>
    public string UserName { get; private set; } = null!;

    /// <summary>
    /// 邮箱.
    /// </summary>
    public string Email { get; private set; } = null!;

    /// <summary>
    /// 密码.
    /// </summary>
    public string Password { get; private set; } = null!;

    /// <summary>
    /// 昵称.
    /// </summary>
    public string NickName { get; private set; } = null!;

    /// <summary>
    /// 头像URL.
    /// </summary>
    public string AvatarUrl { get; private set; } = null!;

    /// <summary>
    /// 手机号.
    /// </summary>
    public string Phone { get; private set; } = null!;

    /// <summary>
    /// 状态：true-正常，false-禁用.
    /// </summary>
    public bool Status { get; private set; }

    /// <summary>
    /// 是否删除.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// JSONB格式的扩展字段.
    /// </summary>
    public string Extensions { get; set; } = null!;

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
