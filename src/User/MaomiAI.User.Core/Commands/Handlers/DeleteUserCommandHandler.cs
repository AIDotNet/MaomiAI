// <copyright file="DeleteUserCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.User.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.User.Core.Commands.Handlers
{
    /// <summary>
    /// 删除用户命令处理程序.
    /// </summary>
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly MaomiaiContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteUserCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">数据库上下文.</param>
        public DeleteUserCommandHandler(MaomiaiContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 处理删除用户命令.
        /// </summary>
        /// <param name="request">命令请求.</param>
        /// <param name="cancellationToken">取消令牌.</param>
        /// <returns>Task.</returns>
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            UserEntity? user = await _dbContext.User
                                   .Where(u => u.Id == request.Id && !u.IsDeleted)
                                   .FirstOrDefaultAsync(cancellationToken)
                               ?? throw new InvalidOperationException($"用户 {request.Id} 不存在或已被删除");

            user.MarkAsDeleted();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}