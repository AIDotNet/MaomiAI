using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 知识库文档处理任务.
/// </summary>
public partial class TeamWikiDocumentTaskConfiguration : IEntityTypeConfiguration<TeamWikiDocumentTaskEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TeamWikiDocumentTaskEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("team_wiki_document_task_pk");

        entity.ToTable("team_wiki_document_task", tb => tb.HasComment("知识库文档处理任务"));

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
        entity.Property(e => e.DocumentId)
            .HasComment("文档id")
            .HasColumnName("document_id");
        entity.Property(e => e.FileId)
            .HasComment("文件id")
            .HasColumnName("file_id");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("软删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.Message)
            .HasDefaultValueSql("''::text")
            .HasComment("执行信息")
            .HasColumnName("message");
        entity.Property(e => e.ModelId)
            .HasComment("当前使用的模型id，默认跟知识库配置一致")
            .HasColumnName("model_id");
        entity.Property(e => e.State)
            .HasComment("任务状态")
            .HasColumnName("state");
        entity.Property(e => e.TaskId)
            .HasMaxLength(50)
            .HasComment("任务标识，用来判断要执行的任务是否一致")
            .HasColumnName("task_id");
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

    partial void OnConfigurePartial(EntityTypeBuilder<TeamWikiDocumentTaskEntity> modelBuilder);
}
