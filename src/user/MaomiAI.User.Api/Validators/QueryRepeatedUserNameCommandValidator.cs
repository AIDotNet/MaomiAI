// <copyright file="QueryRepeatedUserNameCommandValidator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.User.Shared.Queries;

namespace MaomiAI.User.Shared.Commands.Validators;

/// <inheritdoc/>
public class QueryRepeatedUserNameCommandValidator : Validator<QueryRepeatedUserNameCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryRepeatedUserNameCommandValidator"/> class.
    /// </summary>
    public QueryRepeatedUserNameCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().MinimumLength(5).MaximumLength(20).WithMessage("长度 5-30 字符.");
    }
}
