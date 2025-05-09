﻿using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 知识库文档.
/// </summary>
public partial class TeamWikiDocumentConfiguration : IEntityTypeConfiguration<TeamWikiDocumentEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TeamWikiDocumentEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("team_wiki_document_pk");

        entity.ToTable("team_wiki_document", tb => tb.HasComment("知识库文档"));

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建者ID")
            .HasColumnName("create_user_id");
        entity.Property(e => e.FileId)
            .HasComment("文件id")
            .HasColumnName("file_id");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("是否删除")
            .HasColumnName("is_deleted");
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
        entity.Property(e => e.WikiId)
            .HasComment("知识库id")
            .HasColumnName("wiki_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TeamWikiDocumentEntity> modelBuilder);
}
