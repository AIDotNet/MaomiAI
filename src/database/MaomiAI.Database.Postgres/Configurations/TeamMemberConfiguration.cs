using System;
using System.Collections.Generic;
using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 团队成员.
/// </summary>
public partial class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMemberEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TeamMemberEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("team_member_pk");

        entity.ToTable("team_member", tb => tb.HasComment("团队成员"));

        entity.HasIndex(e => new { e.TeamId, e.UserId }, "team_member_pk2");

        entity.Property(e => e.Id)
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人")
            .HasColumnName("create_user_id");
        entity.Property(e => e.IsAdmin)
            .HasDefaultValue(false)
            .HasComment("是否为管理员")
            .HasColumnName("is_admin");
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
        entity.Property(e => e.UserId)
            .HasComment("用户id")
            .HasColumnName("user_id");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TeamMemberEntity> modelBuilder);
}
