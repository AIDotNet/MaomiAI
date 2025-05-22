using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 插件分组.
/// </summary>
public partial class TeamPluginGroupConfiguration : IEntityTypeConfiguration<TeamPluginGroupEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TeamPluginGroupEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("team_plugin_group_pk");

        entity.ToTable("team_plugin_group", tb => tb.HasComment("插件分组"));

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
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasDefaultValueSql("''::character varying")
            .HasComment("描述")
            .HasColumnName("description");
        entity.Property(e => e.Header)
            .HasDefaultValueSql("''::text")
            .HasComment("自定义header头")
            .HasColumnName("header");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("软删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.Name)
            .HasMaxLength(20)
            .HasComment("分组名称")
            .HasColumnName("name");
        entity.Property(e => e.OpenapiFileId)
            .HasComment("文件id，mcp不需要填写")
            .HasColumnName("openapi_file_id");
        entity.Property(e => e.OpenapiFileName)
            .HasMaxLength(50)
            .HasDefaultValueSql("''::character varying")
            .HasComment("openapi文件名称")
            .HasColumnName("openapi_file_name");
        entity.Property(e => e.Server)
            .HasMaxLength(255)
            .HasComment("自定义服务器地址，mcp导入后无法修改")
            .HasColumnName("server");
        entity.Property(e => e.TeamId)
            .HasComment("团队id")
            .HasColumnName("team_id");
        entity.Property(e => e.Type)
            .HasComment("类型，mcp或openapi或system")
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

    partial void OnConfigurePartial(EntityTypeBuilder<TeamPluginGroupEntity> modelBuilder);
}
