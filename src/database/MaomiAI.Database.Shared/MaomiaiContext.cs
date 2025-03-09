// <copyright file="MaomiaiContext.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MaomiAI.Database;

/// <summary>
/// 数据库上下文.
/// </summary>
public partial class MaomiaiContext : DbContext
{
    private readonly Assembly _dbModuleAssembly;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaomiaiContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="dbModuleAssembly">指定初始化配置的模块程序集.</param>
    public MaomiaiContext(DbContextOptions<MaomiaiContext> options, Assembly? dbModuleAssembly = null)
        : base(options)
    {
            _dbModuleAssembly = dbModuleAssembly ?? typeof(MaomiaiContext).Assembly;
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
    /// 团队成员.
    /// </summary>
    public virtual DbSet<TeamMemberEntity> TeamMembers { get; set; }

    /// <summary>
    /// 用户表.
    /// </summary>
    public virtual DbSet<UserEntity> Users { get; set; }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(_dbModuleAssembly);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
