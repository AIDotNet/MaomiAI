using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 提示词.
/// </summary>
public partial class PromptConfiguration : IEntityTypeConfiguration<PromptEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<PromptEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("prompt_pk");

        entity.ToTable("prompt", tb => tb.HasComment("提示词"));

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.AvatarPath)
            .HasMaxLength(255)
            .HasDefaultValueSql("''::character varying")
            .HasComment("头像路径")
            .HasColumnName("avatar_path");
        entity.Property(e => e.Content)
            .HasDefaultValueSql("''::text")
            .HasComment("助手设定,markdown")
            .HasColumnName("content");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人")
            .HasColumnName("create_user_id");
        entity.Property(e => e.Description)
            .HasMaxLength(50)
            .HasDefaultValueSql("''::character varying")
            .HasComment("描述")
            .HasColumnName("description");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("软删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.IsPublic)
            .HasDefaultValue(false)
            .HasComment("是否公开")
            .HasColumnName("is_public");
        entity.Property(e => e.Name)
            .HasMaxLength(20)
            .HasComment("名称")
            .HasColumnName("name");
        entity.Property(e => e.Tags)
            .HasDefaultValueSql("''::text")
            .HasComment("标签，使用逗号\",\"分割多个标签值")
            .HasColumnName("tags");
        entity.Property(e => e.TeamId)
            .HasComment("团队id")
            .HasColumnName("team_id");
        entity.Property(e => e.Type)
            .HasMaxLength(20)
            .HasDefaultValueSql("''::character varying")
            .HasComment("分类")
            .HasColumnName("type");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人")
            .HasColumnName("update_user_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<PromptEntity> modelBuilder);
}
