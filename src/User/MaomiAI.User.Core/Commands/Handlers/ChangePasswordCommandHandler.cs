// <copyright file="ChangePasswordCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.User.Core.Services;
using MaomiAI.User.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.User.Core.Commands.Handlers
{
    /// <summary>
    /// 修改密码命令处理程序.
    /// </summary>
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly MaomiaiContext _dbContext;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        /// <param name="logger">日志记录器.</param>
        public ChangePasswordCommandHandler(MaomiaiContext dbContext, ILogger<ChangePasswordCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// 处理修改密码命令.
        /// </summary>
        /// <param name="request">命令请求.</param>
        /// <param name="cancellationToken">取消令牌.</param>
        /// <returns>Task.</returns>
        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            UserEntity? user = await _dbContext.User.Where(u => u.Id == request.UserId && !u.IsDeleted)
                                   .FirstOrDefaultAsync(cancellationToken)
                               ?? throw new InvalidOperationException($"用户 {request.UserId} 不存在或已被删除");

            // 验证旧密码
            if (!PasswordService.VerifyPassword(request.OldPassword, user.Password))
            {
                _logger.LogWarning("修改密码失败，旧密码不正确: {UserId}", request.UserId);
                throw new InvalidOperationException("旧密码不正确");
            }

            // 新密码不能与旧密码相同
            if (request.OldPassword == request.NewPassword)
            {
                _logger.LogWarning("修改密码失败，新密码与旧密码相同: {UserId}", request.UserId);
                throw new InvalidOperationException("新密码不能与旧密码相同");
            }

            // 使用PasswordService对新密码进行哈希处理
            string newHashedPassword = PasswordService.HashPassword(request.NewPassword);

            // 使用实体的ChangePassword方法更新密码
            user.ChangePassword(newHashedPassword);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("用户密码修改成功: {UserId}", request.UserId);
        }
    }
}