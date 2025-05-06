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

        entity.HasIndex(e => new { e.IsPublic, e.ObjectKey }, "files_is_public_object_key_uindex").IsUnique();

        entity.HasIndex(e => e.ObjectKey, "files_object_key_index");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.ContentType)
            .HasMaxLength(30)
            .HasDefaultValueSql("'application/octet-stream'::character varying")
            .HasComment("文件类型")
            .HasColumnName("content_type");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人")
            .HasColumnName("create_user_id");
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
            .HasComment("软删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.IsPublic)
            .HasDefaultValue(false)
            .HasComment("允许公开访问，公有文件不带路径")
            .HasColumnName("is_public");
        entity.Property(e => e.IsUpload)
            .HasDefaultValue(false)
            .HasComment("已上传文件")
            .HasColumnName("is_upload");
        entity.Property(e => e.ObjectKey)
            .HasMaxLength(255)
            .HasComment("文件路径")
            .HasColumnName("object_key");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新用户id")
            .HasColumnName("update_user_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<FileEntity> modelBuilder);
}
