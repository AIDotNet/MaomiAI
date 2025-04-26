// <copyright file="InternalPreUploadImageCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Commands.Response;
using MediatR;

namespace MaomiAI.Store.InternalCommands;

/// <summary>
/// 上传图片，图片有指定路径等信息.
/// </summary>
public class InternalPreUploadImageCommand : IRequest<PreUploadFileCommandResponse>
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
