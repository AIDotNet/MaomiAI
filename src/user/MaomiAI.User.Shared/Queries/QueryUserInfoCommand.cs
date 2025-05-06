// <copyright file="QueryUserInfoCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.User.Shared.Queries;

/// <summary>
/// 查询用户基本信息的请求.
/// </summary>
public class QueryUserInfoCommand : IRequest<QueryUserInfoCommandResponse>
{
    /// <summary>
    /// 用户 ID.
    /// </summary>
    public Guid UserId { get; init; }
}
