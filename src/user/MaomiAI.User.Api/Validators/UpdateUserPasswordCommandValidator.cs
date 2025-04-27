// <copyright file="LoginCommandValidator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;

namespace MaomiAI.User.Shared.Commands.Validators;

/// <inheritdoc/>
public class UpdateUserPasswordCommandValidator : Validator<UpdateUserPasswordCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserPasswordCommandValidator"/> class.
    /// </summary>
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
    }
}
