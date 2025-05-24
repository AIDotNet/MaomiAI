// <copyright file="UpdateNoteCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Note.Commands;

/// <summary>
/// 更新笔记标题.
/// </summary>
public class UpdateNoteCommand : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// 笔记ID.
    /// </summary>
    public Guid NoteId { get; init; }

    /// <summary>
    /// 标题.
    /// </summary>
    public string? Title { get; init; } = default!;

    /// <summary>
    /// 图标.
    /// </summary>
    public string? TitleEmoji { get; init; } = default!;

    /// <summary>
    /// 摘要.
    /// </summary>
    public string? Summary { get; init; } = default!;

    /// <summary>
    /// 内容.
    /// </summary>
    public string? Content { get; init; } = default!;
}
