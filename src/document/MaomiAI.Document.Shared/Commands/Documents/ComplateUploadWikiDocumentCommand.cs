// <copyright file="ComplateUploadWikiDocumentCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Commands.Response;
using MediatR;

namespace MaomiAI.Document.Shared.Commands.Documents;

/// <summary>
/// 结束上传文件.
/// </summary>
public class ComplateUploadWikiDocumentCommand : IRequest<ComplateFileCommandResponse>
{
    /// <summary>
    /// 团队ID.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 知识库ID.
    /// </summary>
    public Guid WikiId { get; init; } = default!;

    /// <summary>
    /// 上传成功或失败.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 文件ID.
    /// </summary>
    public Guid FileId { get; set; }
}