using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 系统配置.
/// </summary>
public partial class SettingConfiguration : IEntityTypeConfiguration<SettingEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<SettingEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("pk_setting");

        entity.ToTable("setting", tb => tb.HasComment("系统配置"));

        entity.HasIndex(e => e.Name, "ix_setting_name").IsUnique();

        entity.Property(e => e.Id)
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人")
            .HasColumnName("create_user_id");
        entity.Property(e => e.CreatorId)
            .HasDefaultValue(0)
            .HasComment("创建者id")
            .HasColumnName("creator_id");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("软删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.Name)
            .HasMaxLength(20)
            .HasComment("配置名称")
            .HasColumnName("name");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人")
            .HasColumnName("update_user_id");
        entity.Property(e => e.Value)
            .HasDefaultValueSql("''::text")
            .HasComment("配置项值")
            .HasColumnName("value");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<SettingEntity> modelBuilder);
}
