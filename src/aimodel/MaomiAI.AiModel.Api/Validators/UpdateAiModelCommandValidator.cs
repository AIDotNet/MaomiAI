// <copyright file="UpdateAiModelCommandValidator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.AiModel.Shared.Commands;

namespace MaomiAI.AiModel.Api.Validators;

/// <summary>
/// UpdateAiModelCommandValidator.
/// </summary>
public class UpdateAiModelCommandValidator : Validator<UpdateAiModelCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateAiModelCommandValidator"/> class.
    /// </summary>
    public UpdateAiModelCommandValidator()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("团队 ID 不能为空.");

        RuleFor(x => x.ModelId)
            .NotEmpty()
            .WithMessage("模型 ID 不能为空.");

        RuleFor(x => x.Endpoint.DeploymentName)
            .NotEmpty().MaximumLength(50)
            .WithMessage("长度 50 内.");
    }
}
