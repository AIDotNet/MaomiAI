﻿// <copyright file="UpdateUserPasswordCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.User.Shared.Commands;

/// <summary>
/// 重置密码.
/// </summary>
public class UpdateUserPasswordCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 新的密码.
    /// </summary>
    public string Password { get; init; }
}