// <copyright file="MaomiaiContext.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database.Audits;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;

namespace MaomiAI.Database;

/// <summary>
/// 数据库上下文.
/// </summary>
public partial class MaomiaiContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DatabaseOptions _contextOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaomiaiContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="contextOptions"></param>
    public MaomiaiContext(DbContextOptions options, IServiceProvider serviceProvider, DatabaseOptions contextOptions)
        : base(options)
    {
        _serviceProvider = serviceProvider;
        _contextOptions = contextOptions;

        // 配置过滤器.
        ChangeTracker.Tracked += (state, args) =>
        {
            AuditFilter(args);
        };

        ChangeTracker.StateChanged += (state, args) =>
        {
            AuditFilter(args);
        };
    }

    /// <summary>
    /// 系统配置.
    /// </summary>
    public virtual DbSet<SettingEntity> Settings { get; set; }

    /// <summary>
    /// 团队.
    /// </summary>
    public virtual DbSet<TeamEntity> Teams { get; set; }

    /// <summary>
    /// ai模型.
    /// </summary>
    public virtual DbSet<TeamAiModelEntity> TeamAiModels { get; set; }

    /// <summary>
    /// 团队成员.
    /// </summary>
    public virtual DbSet<TeamMemberEntity> TeamMembers { get; set; }

    /// <summary>
    /// 知识库.
    /// </summary>
    public virtual DbSet<TeamWikiEntity> TeamWikis { get; set; }

    /// <summary>
    /// 用户表.
    /// </summary>
    public virtual DbSet<UserEntity> Users { get; set; }

    /// <summary>
    /// 用户表的别名，为了兼容现有代码.
    /// </summary>
    public virtual DbSet<UserEntity> User => Users;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(_contextOptions.ConfigurationAssembly)
            .ApplyConfigurationsFromAssembly(_contextOptions.EntityAssembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

/// <summary>
/// 数据库上下文.
/// </summary>
public partial class MaomiaiContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        SeedData(modelBuilder);

        QueryFilter(modelBuilder);
    }

    // 
    private static void QueryFilter(ModelBuilder modelBuilder)
    {
        // 给实体配置查询时自动加上 IsDeleted == false;
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.IsAssignableTo(typeof(IDeleteAudited)))
            {
                // 构造 x => x.IsDeleted == false
                var parameter = Expression.Parameter(entityType.ClrType, "x");
                MemberExpression property = Expression.Property(parameter, nameof(IDeleteAudited.IsDeleted));
                ConstantExpression constant = Expression.Constant(false);
                BinaryExpression comparison = Expression.Equal(property, constant);

                var lambdaExpression = Expression.Lambda(comparison, parameter);

                entityType.SetQueryFilter(lambdaExpression);
            }
        }
    }

    // 定义种子数据
    private void SeedData(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<UserEntity>().HasData(
        //    new UserEntity
        //    {
        //    });
    }

    // 审计属性过滤
    private void AuditFilter(EntityEntryEventArgs args)
    {
        // todo: 要区分 API 调用和用户调用

        if (args.Entry.State == EntityState.Unchanged)
        {
            return;
        }

        if (args.Entry.State == EntityState.Added && args.Entry.Entity is ICreationAudited creationAudited)
        {
            creationAudited.CreateUserId = default(Guid);
            creationAudited.CreateTime = DateTimeOffset.Now;
        }
        else if (args.Entry.State == EntityState.Modified && args.Entry.Entity is IModificationAudited modificationAudited)
        {
            modificationAudited.UpdateUserId = default(Guid);
            modificationAudited.UpdateTime = DateTimeOffset.Now;
        }
        else if (args.Entry.State == EntityState.Deleted && args.Entry.Entity is IDeleteAudited deleteAudited)
        {
            args.Entry.State = EntityState.Modified;

            deleteAudited.IsDeleted = true;
            deleteAudited.UpdateUserId = default(Guid);
            deleteAudited.UpdateTime = DateTimeOffset.Now;
            args.Entry.CurrentValues[nameof(IDeleteAudited.IsDeleted)] = true;
        }
    }
}