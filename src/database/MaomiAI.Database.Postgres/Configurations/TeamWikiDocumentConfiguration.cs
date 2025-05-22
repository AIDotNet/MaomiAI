using System;
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

        entity.HasIndex(e => e.FileName, "team_wiki_document_file_name_index");

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
        entity.Property(e => e.FileId)
            .HasComment("文件id")
            .HasColumnName("file_id");
        entity.Property(e => e.FileName)
            .HasMaxLength(50)
            .HasDefaultValueSql("''::character varying")
            .HasComment("冗余列，文件名")
            .HasColumnName("file_name");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("软删除")
            .HasColumnName("is_deleted");
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
        entity.Property(e => e.WikiId)
            .HasComment("知识库id")
            .HasColumnName("wiki_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TeamWikiDocumentEntity> modelBuilder);
}
