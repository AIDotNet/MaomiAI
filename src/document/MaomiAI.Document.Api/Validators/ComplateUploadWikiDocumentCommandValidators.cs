// <copyright file="CreateWikiCommandValidators.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.Document.Shared.Commands.Documents;

namespace MaomiAI.Document.Api.Validators;

/// <summary>
/// ComplateUploadWikiDocumentCommandValidators.
/// </summary>
public class ComplateUploadWikiDocumentCommandValidators : Validator<ComplateUploadWikiDocumentCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComplateUploadWikiDocumentCommandValidators"/> class.
    /// </summary>
    public ComplateUploadWikiDocumentCommandValidators()
    {
        RuleFor(x => x.TeamId)
            .NotEmpty()
            .WithMessage("团队ID不能为空");

        RuleFor(x => x.WikiId)
            .NotEmpty();

        RuleFor(x => x.FileId)
            .NotEmpty();
    }
}
