// <copyright file="GetUserByNameQueryHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.User.Shared.Models;
using MaomiAI.User.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.User.Core.Queries.Handlers
{
    /// <summary>
    /// 处理根据用户名获取用户查询.
    /// </summary>
    public class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, UserDto?>
    {
        private readonly MaomiaiContext _dbContext;
        private readonly ILogger<GetUserByNameQueryHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserByNameQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        /// <param name="logger">日志记录器.</param>
        public GetUserByNameQueryHandler(MaomiaiContext dbContext, ILogger<GetUserByNameQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// 处理获取用户查询.
        /// </summary>
        /// <param name="request">查询请求.</param>
        /// <param name="cancellationToken">取消令牌.</param>
        /// <returns>用户信息.</returns>
        public async Task<UserDto?> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("正在查询用户名为 {UserName} 的用户", request.UserName);

                UserEntity? user = await _dbContext.User
                    .Where(u => u.UserName == request.UserName && !u.IsDeleted)
                    .FirstOrDefaultAsync(cancellationToken);

                if (user == null)
                {
                    _logger.LogWarning("未找到用户名为 {UserName} 的用户", request.UserName);
                    return null;
                }

                _logger.LogInformation("成功查询到用户名为 {UserName} 的用户", request.UserName);

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询用户名为 {UserName} 的用户时发生错误", request.UserName);
                throw;
            }
        }
    }
}