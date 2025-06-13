using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 插件.
/// </summary>
public partial class TeamPluginConfiguration : IEntityTypeConfiguration<TeamPluginEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TeamPluginEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("team_plugin_pk");

        entity.ToTable("team_plugin", tb => tb.HasComment("插件"));

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人")
            .HasColumnName("create_user_id");
        entity.Property(e => e.GroupId)
            .HasComment("分组id")
            .HasColumnName("group_id");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("软删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.Name)
            .HasMaxLength(50)
            .HasDefaultValueSql("''::character varying")
            .HasComment("名称，接口名称或mcp名称")
            .HasColumnName("name");
        entity.Property(e => e.Path)
            .HasMaxLength(255)
            .HasDefaultValueSql("''::character varying")
            .HasComment("路径")
            .HasColumnName("path");
        entity.Property(e => e.Summary)
            .HasMaxLength(255)
            .HasDefaultValueSql("''::character varying")
            .HasComment("注释")
            .HasColumnName("summary");
        entity.Property(e => e.TeamId)
            .HasComment("团队id")
            .HasColumnName("team_id");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人")
            .HasColumnName("update_user_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TeamPluginEntity> modelBuilder);
}
