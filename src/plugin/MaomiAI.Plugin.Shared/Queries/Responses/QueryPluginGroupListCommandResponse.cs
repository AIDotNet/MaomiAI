// <copyright file="QueryPluginGroupListCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Plugin.Shared.Queries.Responses;
public class QueryPluginGroupListCommandResponse
{
    public IReadOnlyCollection<QueryPluginGroupListItem> Items { get; init; }
}
