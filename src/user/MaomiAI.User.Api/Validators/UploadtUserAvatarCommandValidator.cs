// <copyright file="LoginCommandValidator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;

namespace MaomiAI.User.Shared.Commands.Validators;

/// <inheritdoc/>
public class UploadtUserAvatarCommandValidator : Validator<UploadtUserAvatarCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UploadtUserAvatarCommandValidator"/> class.
    /// </summary>
    public UploadtUserAvatarCommandValidator()
    {
        RuleFor(x => x.FileId).NotEmpty();
    }
}
