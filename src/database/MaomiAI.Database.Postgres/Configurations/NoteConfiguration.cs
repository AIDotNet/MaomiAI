using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 笔记.
/// </summary>
public partial class NoteConfiguration : IEntityTypeConfiguration<NoteEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<NoteEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("note_pk");

        entity.ToTable("note", tb => tb.HasComment("笔记"));

        entity.HasIndex(e => e.CurrentPath, "note_current_path_index");

        entity.HasIndex(e => e.ParentPath, "note_parent_path_index");

        entity.HasIndex(e => e.NoteId, "note_pk_2").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.Content)
            .HasDefaultValueSql("''::text")
            .HasComment("笔记内容")
            .HasColumnName("content");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人")
            .HasColumnName("create_user_id");
        entity.Property(e => e.CurrentPath)
            .HasMaxLength(512)
            .HasDefaultValueSql("'/root'::character varying")
            .HasComment("当前路径，等于父级路径加上自己的note_id")
            .HasColumnName("current_path");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("软删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.IsShared)
            .HasDefaultValue(false)
            .HasComment("开启共享")
            .HasColumnName("is_shared");
        entity.Property(e => e.NoteId)
            .HasDefaultValue(0)
            .HasComment("自增id，用于标识父级等")
            .HasColumnName("note_id");
        entity.Property(e => e.ParentId)
            .HasComment("父级id")
            .HasColumnName("parent_id");
        entity.Property(e => e.ParentPath)
            .HasMaxLength(512)
            .HasDefaultValueSql("'/root'::character varying")
            .HasComment("父级路径")
            .HasColumnName("parent_path");
        entity.Property(e => e.Summary)
            .HasMaxLength(255)
            .HasComment("总结")
            .HasColumnName("summary");
        entity.Property(e => e.Title)
            .HasMaxLength(50)
            .HasComment("笔记标题")
            .HasColumnName("title");
        entity.Property(e => e.TitleEmoji)
            .HasMaxLength(20)
            .HasComment("图标")
            .HasColumnName("title_emoji");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人")
            .HasColumnName("update_user_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<NoteEntity> modelBuilder);
}
