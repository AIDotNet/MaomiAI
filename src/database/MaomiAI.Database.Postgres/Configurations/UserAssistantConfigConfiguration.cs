using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 用户助手设置.
/// </summary>
public partial class UserAssistantConfigConfiguration : IEntityTypeConfiguration<UserAssistantConfigEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserAssistantConfigEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.UserId).HasName("user_assistant_config_pk");

        entity.ToTable("user_assistant_config", tb => tb.HasComment("用户助手设置"));

        entity.Property(e => e.UserId)
            .ValueGeneratedNever()
            .HasComment("用户id")
            .HasColumnName("user_id");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人")
            .HasColumnName("create_user_id");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasDefaultValueSql("''::character varying")
            .HasComment("助手描述")
            .HasColumnName("description");
        entity.Property(e => e.Icon)
            .HasMaxLength(10)
            .HasDefaultValueSql("'🤖'::character varying")
            .HasComment("头像")
            .HasColumnName("icon");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("软删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.Name)
            .HasMaxLength(10)
            .HasComment("个人助手名字")
            .HasColumnName("name");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人")
            .HasColumnName("update_user_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<UserAssistantConfigEntity> modelBuilder);
}
