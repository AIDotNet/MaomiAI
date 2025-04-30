// <copyright file="UploadWikiAvatarCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Document.Shared.Commands;

/// <summary>
/// 上传知识库头像.
/// </summary>
public class UploadWikiAvatarCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 文件 id.
    /// </summary>
    public Guid FileId { get; init; }
}
