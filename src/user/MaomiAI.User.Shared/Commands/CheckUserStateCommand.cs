// <copyright file="CheckUserStateCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.User.Shared.Commands;

/// <summary>
/// 检查用户状态，判断用户是否存在以及是否被禁用.
/// </summary>
public class CheckUserStateCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 用户 id.
    /// </summary>
    public Guid UserId { get; init; }
}
