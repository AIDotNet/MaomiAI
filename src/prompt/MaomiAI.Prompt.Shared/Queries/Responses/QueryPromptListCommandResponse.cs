// <copyright file="CreatePromptCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MaomiAI.Prompt.Models;
using MediatR;

namespace MaomiAI.Prompt.Queries.Responses;

public class QueryPromptListCommandResponse
{
    public IReadOnlyCollection<PromptItem> Items { get; init; } = new List<PromptItem>();
}
