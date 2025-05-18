// <copyright file="DeleteWikiCommandValidators.cs" company="MaomiAI">
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
/// DeleteWikiCommandValidators.
/// </summary>
public class DeleteWikiCommandValidators : Validator<DeleteWikiCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteWikiCommandValidators"/> class.
    /// </summary>
    public DeleteWikiCommandValidators()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty();
        RuleFor(x => x.WikiId)
            .NotEmpty();
    }
}
