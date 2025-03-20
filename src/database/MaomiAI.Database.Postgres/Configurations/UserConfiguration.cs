using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database
{
    /// <summary>
    /// 用户表.
    /// </summary>
    public partial class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            EntityTypeBuilder<UserEntity>? entity = builder;
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users", tb => tb.HasComment("用户表"));

            entity.HasIndex(e => e.CreateUserId, "idx_users_create_user_id");

            entity.HasIndex(e => e.Email, "idx_users_email");

            entity.HasIndex(e => e.Extensions, "idx_users_extensions").HasMethod("gin");

            entity.HasIndex(e => e.Phone, "idx_users_phone");

            entity.HasIndex(e => e.UpdateUserId, "idx_users_update_user_id");

            entity.HasIndex(e => e.UserName, "idx_users_user_name");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.UserName, "users_user_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasComment("用户ID")
                .HasColumnName("id");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasComment("头像URL")
                .HasColumnName("avatar_url");
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
            entity.Property(e => e.Extensions)
                .HasDefaultValueSql("'{}'::jsonb")
                .HasComment("JSONB格式的扩展字段")
                .HasColumnType("jsonb")
                .HasColumnName("extensions");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasComment("是否删除")
                .HasColumnName("is_deleted");
            entity.Property(e => e.NickName)
                .HasMaxLength(50)
                .HasComment("昵称")
                .HasColumnName("nick_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasComment("密码")
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasComment("手机号")
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasComment("状态：true-正常，false-禁用")
                .HasColumnName("status");
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
}