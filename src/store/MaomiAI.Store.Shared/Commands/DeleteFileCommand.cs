// <copyright file="DeleteFileCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Store.Commands;

/// <summary>
/// 删除文件.
/// </summary>
public class DeleteFileCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 文件id.
    /// </summary>
    public Guid FileId { get; init; }
}
