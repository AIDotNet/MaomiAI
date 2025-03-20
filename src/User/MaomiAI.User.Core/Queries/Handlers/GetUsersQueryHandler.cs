// <copyright file="GetUsersQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.User.Shared.Models;
using MaomiAI.User.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.User.Core.Queries.Handlers
{
    /// <summary>
    /// 处理获取用户列表查询.
    /// </summary>
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedResult<UserDto>>
    {
        private readonly MaomiaiContext _dbContext;
        private readonly ILogger<GetUsersQueryHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUsersQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        /// <param name="logger">日志记录器.</param>
        public GetUsersQueryHandler(MaomiaiContext dbContext, ILogger<GetUsersQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// 处理获取用户列表查询.
        /// </summary>
        /// <param name="request">查询请求.</param>
        /// <param name="cancellationToken">取消令牌.</param>
        /// <returns>用户列表.</returns>
        public async Task<PagedResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Database.Entities.UserEntity> query = _dbContext.User;

            query = ApplyFilters(query, request);

            int totalCount = await query.CountAsync(cancellationToken);

            if (totalCount == 0)
            {
                return new PagedResult<UserDto>(new List<UserDto>(), 0, request.Page, request.PageSize);
            }

            // 应用排序和分页，并映射到DTO
            List<UserDto>? items = await query
                .OrderByDescending(u => u.CreateTime)
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

        /// <summary>
        /// 应用筛选条件
        /// </summary>
        /// <param name="query">基础查询</param>
        /// <param name="request">查询请求</param>
        /// <returns>应用筛选条件后的查询</returns>
        private static IQueryable<Database.Entities.UserEntity> ApplyFilters(
            IQueryable<Database.Entities.UserEntity> query, GetUsersQuery request)
        {
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                string keyword = "%" + request.Keyword.ToUpperInvariant() + "%";

                query = query.Where(u =>
                    EF.Functions.ILike(u.UserName, keyword) ||
                    EF.Functions.ILike(u.NickName, keyword) ||
                    EF.Functions.ILike(u.Email, keyword) ||
                    EF.Functions.ILike(u.Phone, keyword));
            }

            // 状态筛选
            if (request.Status.HasValue)
            {
                query = query.Where(u => u.Status == request.Status.Value);
            }

            return query;
        }
    }
}