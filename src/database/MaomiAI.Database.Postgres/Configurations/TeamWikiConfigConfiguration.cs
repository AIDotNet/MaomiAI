using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 团队知识库配置.
/// </summary>
public partial class TeamWikiConfigConfiguration : IEntityTypeConfiguration<TeamWikiConfigEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TeamWikiConfigEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("team_wiki_config_pk");

        entity.ToTable("team_wiki_config", tb => tb.HasComment("团队知识库配置"));

        entity.HasIndex(e => e.TeamId, "team_wiki_config_team_id_uindex").IsUnique();

        entity.HasIndex(e => e.WikiId, "team_wiki_config_wiki_id_uindex").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建者id")
            .HasColumnName("create_user_id");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("是否删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.ModelId)
            .HasComment("指定进行文档向量化的模型")
            .HasColumnName("model_id");
        entity.Property(e => e.TeamId)
            .HasComment("团队id")
            .HasColumnName("team_id");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人id")
            .HasColumnName("update_user_id");
        entity.Property(e => e.WikiId)
            .HasComment("知识库id")
            .HasColumnName("wiki_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TeamWikiConfigEntity> modelBuilder);
}
