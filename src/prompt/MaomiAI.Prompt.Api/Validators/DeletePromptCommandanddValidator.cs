// <copyright file="CreatePromptCommandValidator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.Prompt.Commands;

namespace MaomiAI.AiModel.Api.Validators;

/// <summary>
/// UpdatePromptCommanddValidator.
/// </summary>
public class DeletePromptCommandanddValidator : Validator<DeletePromptCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePromptCommandanddValidator"/> class.
    /// </summary>
    public DeletePromptCommandanddValidator()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("团队 ID 不能为空.");

        RuleFor(x => x.PromptId)
            .NotEmpty();

    }
}