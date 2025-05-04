// <copyright file="QueryAiModelDefaultConfigurationsCommandValidator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.AiModel.Shared.Queries;

namespace MaomiAI.AiModel.Api.Validators;

/// <summary>
/// QueryAiModelDefaultConfigurationsCommandValidator.
/// </summary>
public class QueryAiModelDefaultConfigurationsCommandValidator : Validator<QueryDefaultAiModelListCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryAiModelDefaultConfigurationsCommandValidator"/> class.
    /// </summary>
    public QueryAiModelDefaultConfigurationsCommandValidator()
    {
        RuleFor(x => x.TeamId).NotEmpty();
    }
}