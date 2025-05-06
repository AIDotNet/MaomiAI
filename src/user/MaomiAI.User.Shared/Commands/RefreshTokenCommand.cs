// <copyright file="RefreshTokenCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.User.Shared.Commands.Responses;
using MediatR;

namespace MaomiAI.User.Shared.Commands;

/// <summary>
/// 刷新 token.
/// </summary>
public class RefreshTokenCommand : IRequest<LoginCommandResponse>
{
    /// <summary>
    /// 刷新令牌.
    /// </summary>
    public string RefreshToken { get; init; } = null!;
}
