// <copyright file="CreateUserCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MaomiAI.User.Shared.Commands
{
    /// <summary>
    /// 创建用户命令.
    /// </summary>
    public class CreateUserCommand : IRequest<Guid>
    {
        /// <summary>
        /// 用户名.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = null!;

        /// <summary>
        /// 邮箱.
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        /// <summary>
        /// 密码.
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        /// <summary>
        /// 昵称.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string NickName { get; set; } = null!;

        /// <summary>
        /// 头像URL.
        /// </summary>
        [StringLength(500)]
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// 手机号.
        /// </summary>
        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        /// <summary>
        /// 状态：true-正常，false-禁用.
        /// </summary>
        public bool Status { get; set; } = true;
    }
}