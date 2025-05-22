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

public class QueryPluginGroupListCommandValidator : Validator<QueryPluginGroupListCommand>
{
    public QueryPluginGroupListCommandValidator()
    {
        RuleFor(x => x.TeamId).NotEmpty().WithMessage("团队id不能为空.");
    }
}
