// <copyright file="UploadTeamAvatarCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FluentValidation;
using MaomiAI.Store.Commands.Response;
using MediatR;

namespace MaomiAI.Team.Shared.Commands;

public class PrivatePreUploadFileCommand : IRequest<PreUploadFileCommandResponse>
{
    /// <summary>
    /// 文件名称.
    /// </summary>
    public string FileName { get; set; } = default!;

    /// <summary>
    /// 文件类型.
    /// </summary>
    public string ContentType { get; set; } = default!;

    /// <summary>
    /// 文件大小.
    /// </summary>
    public long FileSize { get; set; } = default!;

    /// <summary>
    /// 文件 MD5.
    /// </summary>
    public string MD5 { get; set; } = default!;
}

public class PrivatePreUploadFileCommandValidator : AbstractValidator<PrivatePreUploadFileCommand>
{
    public PrivatePreUploadFileCommandValidator()
    {
        RuleFor(x => x.FileName).NotEmpty().WithMessage("文件名称不能为空.");
        RuleFor(x => x.ContentType).NotEmpty().WithMessage("文件类型不能为空.");
        RuleFor(x => x.FileSize).GreaterThan(0).WithMessage("文件大小必须大于0.");
        RuleFor(x => x.MD5).NotEmpty().WithMessage("文件 MD5 不能为空.");
    }
}
