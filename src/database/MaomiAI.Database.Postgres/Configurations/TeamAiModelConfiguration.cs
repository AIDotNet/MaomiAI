using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaomiAI.Database
{
    /// <summary>
    /// ai模型.
    /// </summary>
    public partial class TeamAiModelConfiguration : IEntityTypeConfiguration<TeamAiModelEntity>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<TeamAiModelEntity> builder)
        {
            EntityTypeBuilder<TeamAiModelEntity>? entity = builder;
            entity.HasKey(e => e.Id).HasName("team_ai_model_pk");

            entity.ToTable("team_ai_model", tb => tb.HasComment("ai模型"));

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
            entity.Property(e => e.ModelType)
                .HasComment("模型类型")
                .HasColumnName("model_type");
            entity.Property(e => e.Provider)
                .HasMaxLength(20)
                .HasComment("ai供应商")
                .HasColumnName("provider");
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
}