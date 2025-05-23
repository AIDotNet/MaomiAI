// <copyright file="PreUploadImageCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Commands.Response;
using MaomiAI.Store.Enums;
using MediatR;
using System.Text.Json.Serialization;

namespace MaomiAI.Team.Shared.Commands;

/// <summary>
/// 上传图像，例如头像、公有图像等，文件公开访问，都根路径下.<br />
/// </summary>
public class PreUploadImageCommand : IRequest<PreUploadFileCommandResponse>
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
    /// 文件类型根据具体的功能模块决定.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UploadImageType ImageType { get; set; } = default!;
}
