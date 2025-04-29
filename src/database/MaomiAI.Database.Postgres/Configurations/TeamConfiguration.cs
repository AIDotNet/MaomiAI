using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 团队.
/// </summary>
public partial class TeamConfiguration : IEntityTypeConfiguration<TeamEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TeamEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("team_pk");

        entity.ToTable("team", tb => tb.HasComment("团队"));

        entity.HasIndex(e => e.Name, "team_name_uindex").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.AvatarId)
            .HasComment("团队头像")
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
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasDefaultValueSql("''::character varying")
            .HasComment("团队描述")
            .HasColumnName("description");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("是否删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.IsDisable)
            .HasDefaultValue(false)
            .HasComment("禁用团队")
            .HasColumnName("is_disable");
        entity.Property(e => e.IsPublic)
            .HasComment("是否公开,能够被外部搜索")
            .HasColumnName("is_public");
        entity.Property(e => e.Markdown)
            .HasMaxLength(2000)
            .HasDefaultValueSql("''::character varying")
            .HasComment("团队详细介绍")
            .HasColumnName("markdown");
        entity.Property(e => e.Name)
            .HasMaxLength(20)
            .HasComment("团队名称")
            .HasColumnName("name");
        entity.Property(e => e.OwnerId)
            .HasComment("所有者id")
            .HasColumnName("owner_id");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人ID")
            .HasColumnName("update_user_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TeamEntity> modelBuilder);
}
