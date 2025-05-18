using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 默认模型配置.
/// </summary>
public partial class TeamDefaultAiModelConfiguration : IEntityTypeConfiguration<TeamDefaultAiModelEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TeamDefaultAiModelEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("team_default_ai_model_pk");

        entity.ToTable("team_default_ai_model", tb => tb.HasComment("默认模型配置"));

        entity.HasIndex(e => new { e.ModelId, e.AiModelType }, "team_default_ai_model_model_id_ai_model_type_uindex").IsUnique();

        entity.HasIndex(e => e.TeamId, "team_default_ai_model_team_id_index");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.AiModelType)
            .HasMaxLength(20)
            .HasComment("功能")
            .HasColumnName("ai_model_type");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人ID")
            .HasColumnName("create_user_id");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("是否删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.ModelId)
            .HasComment("模型id")
            .HasColumnName("model_id");
        entity.Property(e => e.TeamId)
            .HasComment("团队id")
            .HasColumnName("team_id");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人ID")
            .HasColumnName("update_user_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TeamDefaultAiModelEntity> modelBuilder);
}
