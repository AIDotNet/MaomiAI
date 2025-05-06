// <copyright file="UploadtUserAvatarCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.User.Shared.Commands;

/// <summary>
/// 上传用户头像.
/// </summary>
public class UploadtUserAvatarCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 文件id.
    /// </summary>
    public Guid FileId { get; init; }
}
