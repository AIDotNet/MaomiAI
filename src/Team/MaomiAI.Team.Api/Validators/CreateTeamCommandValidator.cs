// <copyright file="CreateTeamCommandValidator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.Team.Shared.Commands.Root;

namespace MaomiAI.Team.Shared.Validators;

public class CreateTeamCommandValidator : Validator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("团队名称最大长度20.");
        RuleFor(x => x.Description).MaximumLength(255).WithMessage("团队描述最大长度255.");
    }
}
