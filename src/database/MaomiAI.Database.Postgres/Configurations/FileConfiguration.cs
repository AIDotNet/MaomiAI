using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 文件列表.
/// </summary>
public partial class FileConfiguration : IEntityTypeConfiguration<FileEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<FileEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("files_pk");

        entity.ToTable("files", tb => tb.HasComment("文件列表"));

        entity.HasIndex(e => e.FileMd5, "files_file_md5_index");

        entity.HasIndex(e => e.FileName, "files_file_name_index");

        entity.HasIndex(e => e.Path, "files_path_index");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
        entity.Property(e => e.FileMd5)
            .HasMaxLength(50)
            .HasComment("文件md5值")
            .HasColumnName("file_md5");
        entity.Property(e => e.FileName)
            .HasMaxLength(100)
            .HasComment("文件名称")
            .HasColumnName("file_name");
        entity.Property(e => e.FileSize)
            .HasComment("文件大小")
            .HasColumnName("file_size");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");
        entity.Property(e => e.IsPublic)
            .HasDefaultValue(false)
            .HasComment("允许公开访问")
            .HasColumnName("is_public");
        entity.Property(e => e.Path)
            .HasMaxLength(255)
            .HasComment("文件路径")
            .HasColumnName("path");
        entity.Property(e => e.SourceType)
            .HasMaxLength(10)
            .HasComment("该文件属于哪个模块")
            .HasColumnName("source_type");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<FileEntity> modelBuilder);
}
