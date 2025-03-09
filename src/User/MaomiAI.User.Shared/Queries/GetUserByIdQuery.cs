// <copyright file="GetUserByIdQuery.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.User.Shared.Models;

namespace MaomiAI.User.Shared.Queries;

/// <summary>
/// 通过ID获取用户查询.
/// </summary>
public class GetUserByIdQuery : IRequest<UserDto?>
{
    /// <summary>
    /// 用户ID.
    /// </summary>
    public Guid Id { get; set; }
} 