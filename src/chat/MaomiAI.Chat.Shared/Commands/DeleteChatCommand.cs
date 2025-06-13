// <copyright file="Class1.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Chat.Shared.Commands.Responses;
using MaomiAI.Chat.Shared.Helpers;
using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Database.Helper;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Team.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MaomiAI.Chat.Core.Handlers;

public class DeleteChatCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 对话 id.
    /// </summary>
    public Guid ChatId { get; init; }
}