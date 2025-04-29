// <copyright file="CheckUserStateCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.User.Shared.Commands;

public class CheckUserStateCommand : IRequest<EmptyCommandResponse>
{
    public Guid UserId { get; init; }
}
