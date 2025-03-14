// <copyright file="GetUserByNameQuery.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.User.Shared.Models;

using MediatR;

namespace MaomiAI.User.Shared.Queries;

/// <summary>
/// 通过用户名获取用户查询.
/// </summary>
public class GetUserByNameQuery : IRequest<UserDto?>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByNameQuery"/> class.
    /// </summary>
    /// <param name="userName">用户名.</param>
    public GetUserByNameQuery(string userName)
    {
        UserName = userName;
    }

    /// <summary>
    /// 用户名.
    /// </summary>
    public string UserName { get; }
}
