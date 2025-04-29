using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 用户表.
/// </summary>
public partial class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("users_pkey");

        entity.ToTable("users", tb => tb.HasComment("用户表"));

        entity.HasIndex(e => e.Email, "idx_users_email").IsUnique();

        entity.HasIndex(e => e.Phone, "idx_users_phone").IsUnique();

        entity.HasIndex(e => e.UserName, "idx_users_user_name").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("用户ID")
            .HasColumnName("id");
        entity.Property(e => e.AvatarId)
            .HasComment("头像id")
            .HasColumnName("avatar_id");
        entity.Property(e => e.AvatarPath)
            .HasMaxLength(255)
            .HasDefaultValueSql("''::character varying")
            .HasComment("头像路径")
            .HasColumnName("avatar_path");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人ID")
            .HasColumnName("create_user_id");
        entity.Property(e => e.Email)
            .HasMaxLength(255)
            .HasComment("邮箱")
            .HasColumnName("email");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("是否删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.IsEnable)
            .HasDefaultValue(true)
            .HasComment("是否启用")
            .HasColumnName("is_enable");
        entity.Property(e => e.NickName)
            .HasMaxLength(50)
            .HasComment("昵称")
            .HasColumnName("nick_name");
        entity.Property(e => e.Password)
            .HasMaxLength(300)
            .HasComment("密码")
            .HasColumnName("password");
        entity.Property(e => e.PasswordHalt)
            .HasMaxLength(300)
            .HasComment("计算密码值的salt")
            .HasColumnName("password_halt");
        entity.Property(e => e.Phone)
            .HasMaxLength(20)
            .HasComment("手机号")
            .HasColumnName("phone");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人ID")
            .HasColumnName("update_user_id");
        entity.Property(e => e.UserName)
            .HasMaxLength(50)
            .HasComment("用户名")
            .HasColumnName("user_name");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<UserEntity> modelBuilder);
}
