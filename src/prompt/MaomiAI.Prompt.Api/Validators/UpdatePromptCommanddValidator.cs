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
public class UpdatePromptCommanddValidator : Validator<UpdatePromptCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePromptCommanddValidator"/> class.
    /// </summary>
    public UpdatePromptCommanddValidator()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("团队 ID 不能为空.");

        RuleFor(x => x.Name).Length(1, 20);
        RuleFor(x => x.Description).Length(1, 255);
        RuleFor(x => x.Content).Length(1, 2000);
        RuleFor(x => x.Tags)
            .Must(x => x.Count <= 10)
            .WithMessage("标签数量不能超过 10 个.")
            .ForEach(x => x.Length(1, 20))
            .WithMessage("标签长度不能超过 20 个字符.");
    }
}

/// <summary>
/// UpdatePromptCommanddValidator.
/// </summary>
public class PublishPromptCommandValidator : Validator<PublishPromptCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PublishPromptCommandValidator"/> class.
    /// </summary>
    public PublishPromptCommandValidator()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("团队 ID 不能为空.");
    }
}
