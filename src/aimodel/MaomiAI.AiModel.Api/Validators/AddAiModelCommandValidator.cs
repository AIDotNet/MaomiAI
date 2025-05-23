﻿// <copyright file="AddAiModelCommandValidator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.AiModel.Api.Models;
using MaomiAI.AiModel.Shared.Commands;
using MaomiAI.AiModel.Shared.Queries;

namespace MaomiAI.AiModel.Api.Validators;

/// <summary>
/// AddAiModelCommandValidator.
/// </summary>
public class AddAiEndpointRequestValidator : Validator<AddAiEndpointRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddAiEndpointRequestValidator"/> class.
    /// </summary>
    public AddAiEndpointRequestValidator()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("团队 ID 不能为空.");

        RuleFor(x => x.Endpoint)
            .NotNull()
            .WithMessage("模型信息不能为空.");

        RuleFor(x => x.DeploymentName)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.DisplayName)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.ContextWindowTokens)
            .GreaterThan(0);

        RuleFor(x => x.Endpoint)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.Key)
            .NotEmpty().MaximumLength(1000);
    }
}