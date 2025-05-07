// <copyright file="LoginCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.User.Shared.Commands.Responses;
using MediatR;

namespace MaomiAI.User.Shared.Commands;

/// <summary>
/// 登录.
/// </summary>
public class LoginCommand : IRequest<LoginCommandResponse>
{
    /// <summary>
    /// 用户名或邮箱.
    /// </summary>
    public string UserName { get; init; } = default!;

    /// <summary>
    /// 密码，使用 RSA 公钥加密.
    /// </summary>
    public string Password { get; init; } = default!;
}
