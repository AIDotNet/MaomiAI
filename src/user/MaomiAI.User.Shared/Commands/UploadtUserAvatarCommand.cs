// <copyright file="UploadtUserAvatarCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.User.Shared.Commands;

/// <summary>
/// 上传用户头像.
/// </summary>
public class UploadtUserAvatarCommand : IRequest<EmptyCommandResponse>
{
    public Guid FileId { get; init; }
}
