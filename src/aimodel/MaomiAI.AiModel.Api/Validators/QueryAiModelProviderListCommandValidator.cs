// <copyright file="QueryAiModelProviderListCommandValidator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.AiModel.Shared.Queries;

namespace MaomiAI.AiModel.Api.Validators;

/// <summary>
/// QueryAiModelProviderListCommandValidator.
/// </summary>
public class QueryAiModelProviderListCommandValidator : Validator<QueryAiModelProviderListCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryAiModelProviderListCommandValidator"/> class.
    /// </summary>
    public QueryAiModelProviderListCommandValidator()
    {
        RuleFor(x => x.TeamId)
            .Equal(default(Guid))
            .WithMessage("团队 ID 不能为空.");
    }
}
