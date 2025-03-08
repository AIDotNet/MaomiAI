using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Database;

public partial class MaomiaiContext : DbContext
{
    public MaomiaiContext()
    {
    }

    public MaomiaiContext(DbContextOptions<MaomiaiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamMember> TeamMembers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Database=maomiai;Host=192.168.3.38;Password=19971120;Port=5432;Username=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_setting");

            entity.ToTable("setting", tb => tb.HasComment("系统配置"));

            entity.HasIndex(e => e.Name, "ix_setting_name").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("id")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasComment("配置名称")
                .HasColumnName("name");
            entity.Property(e => e.Value)
                .HasDefaultValueSql("''::text")
                .HasComment("配置项值")
                .HasColumnName("value");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("team_pk");

            entity.ToTable("team", tb => tb.HasComment("团队"));

            entity.Property(e => e.Id)
                .HasComment("id")
                .HasColumnName("id");
            entity.Property(e => e.Avatar)
                .HasMaxLength(100)
                .HasComment("团队头像")
                .HasColumnName("avatar");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("''::character varying")
                .HasComment("团队描述")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasComment("团队名称")
                .HasColumnName("name");
            entity.Property(e => e.RootId)
                .HasDefaultValue(0)
                .HasComment("超级管理员id")
                .HasColumnName("root_id");
        });

        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_team_member");

            entity.ToTable("team_member", tb => tb.HasComment("团队成员"));

            entity.HasIndex(e => new { e.TeamId, e.UserId }, "ix_team_member_teamid_userid").IsUnique();

            entity.Property(e => e.Id)
                .HasComment("id")
                .HasColumnName("id");
            entity.Property(e => e.IsAdmin)
                .HasComment("是否管理员")
                .HasColumnName("is_admin");
            entity.Property(e => e.TeamId)
                .HasComment("团队id")
                .HasColumnName("team_id");
            entity.Property(e => e.UserId)
                .HasComment("用户id")
                .HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
