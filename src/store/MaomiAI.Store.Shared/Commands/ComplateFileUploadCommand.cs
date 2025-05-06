// <copyright file="ComplateFileUploadCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FluentValidation;
using MaomiAI.Store.Commands.Response;
using MediatR;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 结束上传文件.
/// </summary>
public class ComplateFileUploadCommand : IRequest<ComplateFileCommandResponse>
{
    /// <summary>
    /// 上传成功或失败.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 文件ID.
    /// </summary>
    public Guid FileId { get; set; }
}
