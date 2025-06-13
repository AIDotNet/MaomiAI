using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 用户对话.
/// </summary>
public partial class UserChatConfiguration : IEntityTypeConfiguration<UserChatEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<UserChatEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("user_chat_pk");

        entity.ToTable("user_chat", tb => tb.HasComment("用户对话"));

        entity.HasIndex(e => e.CreateUserId, "user_chat_create_user_id_index");

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
        entity.Property(e => e.FrequenctPenalty)
            .HasComment("频率惩罚度,值越大，越有可能降低重复字词,-2.0-2.0")
            .HasColumnName("frequenct_penalty");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("软删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.PresencePenalty)
            .HasComment("话题新鲜度，值越大，越有可能扩展到新话题,-2.0-2.0")
            .HasColumnName("presence_penalty");
        entity.Property(e => e.Prompt)
            .HasMaxLength(5000)
            .HasComment("提示词，角色设定")
            .HasColumnName("prompt");
        entity.Property(e => e.TeamId)
            .HasComment("团队id，要标识用户在哪个团队上下文")
            .HasColumnName("tema_id");
        entity.Property(e => e.Temperature)
            .HasDefaultValueSql("1.0")
            .HasComment("随机性，值越大，回复越随机,0-2.0")
            .HasColumnName("temperature");
        entity.Property(e => e.Title)
            .HasMaxLength(100)
            .HasDefaultValueSql("''::character varying")
            .HasComment("话题标题")
            .HasColumnName("title");
        entity.Property(e => e.TopP)
            .HasDefaultValueSql("1.0")
            .HasComment("核采样，与随机性类似，但不要和随机性一起更改,0-1.0")
            .HasColumnName("top_p");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人")
            .HasColumnName("update_user_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<UserChatEntity> modelBuilder);
}
