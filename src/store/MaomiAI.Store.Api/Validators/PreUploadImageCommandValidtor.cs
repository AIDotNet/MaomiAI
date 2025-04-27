// <copyright file="PreUploadImageCommandValidtor.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using FluentValidation;
using MaomiAI.Store.Commands;
using MaomiAI.Team.Shared.Commands;

namespace MaomiAI.Store.Validators;

public class PreUploadImageCommandValidtor : Validator<PreUploadImageCommand>
{
    public PreUploadImageCommandValidtor()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("文件名称不能为空")
            .MaximumLength(100)
            .WithMessage("文件名称长度不能超过100个字符");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .WithMessage("文件类型不能为空")
            .MaximumLength(30)
            .WithMessage("文件类型长度不能超过30个字符");

        RuleFor(x => x.MD5)
            .NotEmpty()
            .WithMessage("MD5不能为空")
            .MaximumLength(50)
            .WithMessage("MD5长度不能超过30个字符");
    }
}
