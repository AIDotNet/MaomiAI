using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 站内信.
/// </summary>
public partial class MessageConfiguration : IEntityTypeConfiguration<MessageEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("message_pk");

        entity.ToTable("message", tb => tb.HasComment("站内信"));

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.Content)
            .HasMaxLength(2000)
            .HasComment("内容")
            .HasColumnName("content");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人")
            .HasColumnName("create_user_id");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("软删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.IsRead)
            .HasDefaultValue(false)
            .HasComment("已读")
            .HasColumnName("is_read");
        entity.Property(e => e.MessageType)
            .HasComment("消息类型")
            .HasColumnName("message_type");
        entity.Property(e => e.ReceiveObjectType)
            .HasComment("接收对象类型")
            .HasColumnName("receive_object_type");
        entity.Property(e => e.RecevieObjectId)
            .HasComment("接收对象id")
            .HasColumnName("recevie_object_id");
        entity.Property(e => e.SendObjectId)
            .HasComment("发送对象id")
            .HasColumnName("send_object_id");
        entity.Property(e => e.SendObjectType)
            .HasComment("发送对象类型")
            .HasColumnName("send_object_type");
        entity.Property(e => e.Title)
            .HasMaxLength(50)
            .HasComment("主题")
            .HasColumnName("title");
        entity.Property(e => e.UpdateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("更新时间")
            .HasColumnName("update_time");
        entity.Property(e => e.UpdateUserId)
            .HasComment("更新人")
            .HasColumnName("update_user_id");
        entity.Property(e => e.Url)
            .HasMaxLength(255)
            .HasDefaultValueSql("''::character varying")
            .HasComment("附带的地址")
            .HasColumnName("url");
        entity.Property(e => e.UrlTitle)
            .HasMaxLength(20)
            .HasDefaultValueSql("''::character varying")
            .HasComment("地址标题")
            .HasColumnName("url_title");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<MessageEntity> modelBuilder);
}
