// <copyright file="CheckFileExistCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Enums;
using MaomiAI.Store.Queries.Response;
using MediatR;

namespace MaomiAI.Store.Queries;

/// <summary>
/// 检查文件是否存在
/// </summary>
public class CheckFileExistCommand : IRequest<CheckFileExistCommandResponse>
{
    /// <summary>
    /// 文件可见性.
    /// </summary>
    public FileVisibility Visibility { get; init; }

    public Guid? FileId { get; init; }
    public string? MD5 { get; init; }
    public string? Key { get; init; }
}
