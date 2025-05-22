// <copyright file="PreUploadOpenApiFileCommandValidator.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.Plugin.Shared.Commands;
using MaomiAI.Plugin.Shared.Queries;
using MaomiAI.Store.Commands;

namespace MaomiAI.Store.Validators;

public class UpdatePluginInfoCommandValidator : Validator<UpdatePluginInfoCommand>
{
    public UpdatePluginInfoCommandValidator()
    {
        RuleFor(x => x.TeamId).NotEmpty().WithMessage("团队id不能为空.");
        RuleFor(x => x.ServerUrl).NotEmpty().Length(2, 255);
        RuleFor(x => x.Name).NotEmpty().Length(2, 20);
    }
}
