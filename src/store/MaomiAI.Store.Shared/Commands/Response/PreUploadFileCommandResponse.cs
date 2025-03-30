// <copyright file="PreuploadFileCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Store.Commands.Response;

/// <summary>
/// 预上传文件.
/// </summary>
public class PreUploadFileCommandResponse
{
    /// <summary>
    /// 文件已存在.
    /// </summary>
    public bool IsExist { get; set; }

    /// <summary>
    /// 文件ID.
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// 预签名上传地址，当 IsExist = true 时字段为空.
    /// </summary>
    public Uri? UploadUrl { get; set; } = default!;

    /// <summary>
    /// 签名过期时间，当 IsExist = true 时字段为空.
    /// </summary>
    public DateTimeOffset? Expiration { get; set; } = default!;
}
