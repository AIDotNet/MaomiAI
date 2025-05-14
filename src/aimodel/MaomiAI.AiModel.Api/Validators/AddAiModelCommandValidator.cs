// <copyright file="AddAiModelCommandValidator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.AiModel.Shared.Commands;

namespace MaomiAI.AiModel.Api.Validators;

/// <summary>
/// AddAiModelCommandValidator.
/// </summary>
public class AddAiModelCommandValidator : Validator<AddAiModelCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddAiModelCommandValidator"/> class.
    /// </summary>
    public AddAiModelCommandValidator()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("团队 ID 不能为空.");

        RuleFor(x => x.Endpoint.Endpoint)
            .NotEmpty().MaximumLength(50)
            .WithMessage("长度 50 内.");

        RuleFor(x => x.Endpoint.ModelId)
            .NotEmpty().MaximumLength(50)
            .WithMessage("长度 50 内.");

        RuleFor(x => x.Endpoint.DeploymentName)
            .NotEmpty().MaximumLength(50)
            .WithMessage("长度 50 内.");

        RuleFor(x => x.Endpoint.Key)
            .NotEmpty().MaximumLength(1000)
            .WithMessage("长度太大.");
    }
}
