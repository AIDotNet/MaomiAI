// <copyright file="CreateNoteCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MediatR;

namespace MaomiAI.Note.Commands;

public class CreateNoteCommand : IRequest<IdResponse>
{
    /// <summary>
    /// 父级笔记ID.
    /// </summary>
    public Guid? ParentNoteId { get; init; }

    /// <summary>
    /// 标题.
    /// </summary>
    public string Title { get; init; } = default!;

    /// <summary>
    /// 图标.
    /// </summary>
    public string? TitleEmoji { get; init; }

    /// <summary>
    /// 内容.
    /// </summary>
    public string? Content { get; init; }
}
