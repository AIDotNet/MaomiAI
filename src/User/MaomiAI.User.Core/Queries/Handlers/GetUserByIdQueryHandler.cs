// <copyright file="GetUserByIdQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.User.Shared;
using MaomiAI.User.Shared.Models;
using MaomiAI.User.Shared.Queries;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.User.Core.Queries.Handlers;

/// <summary>
/// 处理根据ID获取用户查询.
/// </summary>
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly MaomiaiContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    public GetUserByIdQueryHandler(MaomiaiContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 处理获取用户查询.
    /// </summary>
    /// <param name="request">查询请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>用户信息.</returns>
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.User
                                  .Where(u => u.Id == request.Id && !u.IsDeleted)
                                  .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            NickName = user.NickName,
            AvatarUrl = user.AvatarUrl,
            Phone = user.Phone,
            Status = user.Status,
            CreateTime = user.CreateTime,
            UpdateTime = user.UpdateTime
        };
    }
} 