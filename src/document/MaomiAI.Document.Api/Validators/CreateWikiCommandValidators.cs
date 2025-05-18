// <copyright file="CreateWikiCommandValidators.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.Document.Shared.Commands;
using MaomiAI.Document.Shared.Commands.Documents;

namespace MaomiAI.Document.Api.Validators;

/// <summary>
/// CreateWikiCommandValidators.
/// </summary>
public class CreateWikiCommandValidators : Validator<CreateWikiCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateWikiCommandValidators"/> class.
    /// </summary>
    public CreateWikiCommandValidators()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("团队ID不能为空");
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(255);
    }
}
