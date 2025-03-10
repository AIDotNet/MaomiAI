// <copyright file="GetUsersQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.User.Shared;
using MaomiAI.User.Shared.Models;
using MaomiAI.User.Shared.Queries;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace MaomiAI.User.Core.Queries.Handlers;

/// <summary>
/// 处理获取用户列表查询.
/// </summary>
public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedResult<UserDto>>
{
    private readonly MaomiaiContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUsersQueryHandler"/> class.
    /// </summary>
    /// <param name="dbContext">数据库上下文.</param>
    public GetUsersQueryHandler(MaomiaiContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// 处理获取用户列表查询.
    /// </summary>
    /// <param name="request">查询请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>用户列表.</returns>
    public async Task<PagedResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Database.Entities.UserEntity> query = _dbContext.User.Where(u => !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keyword = request.Keyword.Trim().ToLower();
            query = query.Where(u => u.UserName.ToLower().Contains(keyword) ||
                                    u.Email.ToLower().Contains(keyword) ||
                                    u.NickName.ToLower().Contains(keyword));
        }

        if (request.Status.HasValue)
        {
            query = query.Where(u => u.Status == request.Status.Value);
        }

        var totalCount = await query.LongCountAsync(cancellationToken);
        var items = await query.OrderByDescending(u => u.CreateTime)
                              .Skip((request.Page - 1) * request.PageSize)
                              .Take(request.PageSize)
                              .Select(u => new UserDto
                              {
                                  Id = u.Id,
                                  UserName = u.UserName,
                                  Email = u.Email,
                                  NickName = u.NickName,
                                  AvatarUrl = u.AvatarUrl,
                                  Phone = u.Phone,
                                  Status = u.Status,
                                  CreateTime = u.CreateTime,
                                  UpdateTime = u.UpdateTime
                              })
                              .ToListAsync(cancellationToken);

        return new PagedResult<UserDto>(items, totalCount, request.Page, request.PageSize);
    }
} 