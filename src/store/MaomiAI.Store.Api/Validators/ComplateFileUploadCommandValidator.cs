// <copyright file="ComplateUploadEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.Store.Commands;

namespace MaomiAI.Store.Validators;

public class ComplateFileUploadCommandValidator : Validator<ComplateFileUploadCommand>
{
    public ComplateFileUploadCommandValidator()
    {
        RuleFor(x => x.FileId).NotEmpty().WithMessage("文件ID不能为空.");
    }
}
