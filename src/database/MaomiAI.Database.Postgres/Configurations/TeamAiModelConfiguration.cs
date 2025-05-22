using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// ai模型.
/// </summary>
public partial class TeamAiModelConfiguration : IEntityTypeConfiguration<TeamAiModelEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TeamAiModelEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("team_ai_model_pk");

        entity.ToTable("team_ai_model", tb => tb.HasComment("ai模型"));

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.AiModelType)
            .HasMaxLength(20)
            .HasDefaultValueSql("'custom'::character varying")
            .HasComment("模型类型，AiModelType")
            .HasColumnName("ai_model_type");
        entity.Property(e => e.AiProvider)
            .HasMaxLength(50)
            .HasDefaultValueSql("'Custom'::character varying")
            .HasComment("ai供应商AiProvider")
            .HasColumnName("ai_provider");
        entity.Property(e => e.ContextWindowTokens)
            .HasDefaultValue(0)
            .HasComment("上下文最大token数量")
            .HasColumnName("context_window_tokens");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人")
            .HasColumnName("create_user_id");
        entity.Property(e => e.DeploymentName)
            .HasMaxLength(50)
            .HasComment("部署名称")
            .HasColumnName("deployment_name");
        entity.Property(e => e.DisplayName)
            .HasMaxLength(20)
            .HasComment("显示名称")
            .HasColumnName("display_name");
        entity.Property(e => e.Endpoint)
            .HasMaxLength(100)
            .HasComment("api服务端点")
            .HasColumnName("endpoint");
        entity.Property(e => e.Files)
            .HasComment("支持文件上传")
            .HasColumnName("files");
        entity.Property(e => e.FunctionCall)
            .HasDefaultValue(false)
            .HasComment("支持function call")
            .HasColumnName("function_call");
        entity.Property(e => e.ImageOutput)
            .HasComment("支持图片输出")
            .HasColumnName("image_output");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("软删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.Key)
            .HasMaxLength(100)
            .HasComment("key")
            .HasColumnName("key");
        entity.Property(e => e.MaxDimension)
            .HasDefaultValue(8191)
            .HasComment("向量的维度")
            .HasColumnName("max_dimension");
        entity.Property(e => e.Name)
            .HasMaxLength(20)
            .HasComment("名字")
            .HasColumnName("name");
        entity.Property(e => e.TeamId)
            .HasComment("团队id")
            .HasColumnName("team_id");
        entity.Property(e => e.TextOutput)
            .HasDefaultValue(8192)
            .HasComment("最大文本输出token")
            .HasColumnName("text_output");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人")
            .HasColumnName("update_user_id");
        entity.Property(e => e.Vision)
            .HasDefaultValue(false)
            .HasComment("支持视觉")
            .HasColumnName("vision");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TeamAiModelEntity> modelBuilder);
}
