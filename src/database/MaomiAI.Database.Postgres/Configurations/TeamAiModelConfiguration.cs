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
        entity.Property(e => e.AiModelFunction)
            .HasComment("模型功能AiModelFunction")
            .HasColumnName("ai_model_function");
        entity.Property(e => e.AiProvider)
            .HasMaxLength(50)
            .HasDefaultValueSql("'Custom'::character varying")
            .HasComment("ai供应商AiProvider")
            .HasColumnName("ai_provider");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人ID")
            .HasColumnName("create_user_id");
        entity.Property(e => e.DeploymentName)
            .HasMaxLength(50)
            .HasComment("部署名称")
            .HasColumnName("deployment_name");
        entity.Property(e => e.Endpoint)
            .HasMaxLength(100)
            .HasComment("api服务端点")
            .HasColumnName("endpoint");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("是否删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.IsSupportFunctionCall)
            .HasDefaultValue(false)
            .HasComment("支持function call")
            .HasColumnName("is_support_function_call");
        entity.Property(e => e.IsSupportImg)
            .HasDefaultValue(false)
            .HasComment("是否支持图片")
            .HasColumnName("is_support_img");
        entity.Property(e => e.Key)
            .HasMaxLength(100)
            .HasComment("key")
            .HasColumnName("key");
        entity.Property(e => e.ModeId)
            .HasMaxLength(50)
            .HasComment("模型id")
            .HasColumnName("mode_id");
        entity.Property(e => e.Name)
            .HasMaxLength(20)
            .HasComment("名字")
            .HasColumnName("name");
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

    partial void OnConfigurePartial(EntityTypeBuilder<TeamAiModelEntity> modelBuilder);
}
