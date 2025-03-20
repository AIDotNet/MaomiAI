// <copyright file="DeleteUserCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.User.Shared.Commands
{
    /// <summary>
    /// 删除用户命令.
    /// </summary>
    public class DeleteUserCommand : IRequest
    {
        /// <summary>
        /// 用户ID.
        /// </summary>
        public Guid Id { get; set; }
    }
}