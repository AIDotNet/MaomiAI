// <copyright file="InternalPreUploadDocumentFileCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FluentValidation;
using MaomiAI.Store.Commands.Response;
using MediatR;

namespace MaomiAI.Store.InternalCommands;

/// <summary>
/// 预上传文档文件.
/// </summary>
public class InternalPreUploadDocumentFileCommand : IRequest<PreUploadFileCommandResponse>
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
    public int FileSize { get; set; } = default!;

    /// <summary>
    /// 文件 MD5.
    /// </summary>
    public string MD5 { get; set; } = default!;

    /// <summary>
    /// 文件路径，即 ObjectKey.
    /// </summary>
    public string Path { get; set; } = null!;

    /// <summary>
    /// 预签名上传地址有效期.
    /// </summary>
    public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(2)!;
}

public class PrivatePreUploadFileCommandValidator : AbstractValidator<InternalPreUploadDocumentFileCommand>
{
    public PrivatePreUploadFileCommandValidator()
    {
        RuleFor(x => x.FileName).NotEmpty().WithMessage("文件名称不能为空.");
        RuleFor(x => x.ContentType).NotEmpty().WithMessage("文件类型不能为空.");
        RuleFor(x => x.FileSize).GreaterThan(0).WithMessage("文件大小必须大于0.");
        RuleFor(x => x.MD5).NotEmpty().WithMessage("文件 MD5 不能为空.");
    }
}
