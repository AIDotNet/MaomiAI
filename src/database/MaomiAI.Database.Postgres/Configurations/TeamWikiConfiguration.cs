using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database;

/// <summary>
/// 知识库.
/// </summary>
public partial class TeamWikiConfiguration : IEntityTypeConfiguration<TeamWikiEntity>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TeamWikiEntity> builder)
    {
        var entity = builder;
        entity.HasKey(e => e.Id).HasName("wiki_pk");

        entity.ToTable("team_wiki", tb => tb.HasComment("知识库"));

        entity.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasComment("id")
            .HasColumnName("id");
        entity.Property(e => e.CreateTime)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasComment("创建时间")
            .HasColumnName("create_time");
        entity.Property(e => e.CreateUserId)
            .HasComment("创建人ID")
            .HasColumnName("create_user_id");
        entity.Property(e => e.Description)
            .HasMaxLength(255)
            .HasDefaultValueSql("''::character varying")
            .HasComment("知识库描述")
            .HasColumnName("description");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValue(false)
            .HasComment("是否删除")
            .HasColumnName("is_deleted");
        entity.Property(e => e.IsPublic)
            .HasDefaultValue(false)
            .HasComment("公开使用，所有人不需要加入团队即可使用此知识库")
            .HasColumnName("is_public");
        entity.Property(e => e.Markdown)
            .HasMaxLength(2000)
            .HasDefaultValueSql("''::character varying")
            .HasComment("知识库详细介绍")
            .HasColumnName("markdown");
        entity.Property(e => e.ModelId)
            .HasComment("绑定的向量模型id")
            .HasColumnName("model_id");
        entity.Property(e => e.Name)
            .HasMaxLength(20)
            .HasComment("知识库名称")
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

    partial void OnConfigurePartial(EntityTypeBuilder<TeamWikiEntity> modelBuilder);
}
