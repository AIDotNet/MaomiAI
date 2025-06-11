// <copyright file="QueryNoteCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Note.Queries.Models;
using MediatR;

namespace MaomiAI.Note.Queries;

/// <summary>
/// 查看笔记内容.
/// </summary>
public class QueryNoteCommand : IRequest<QueryNoteCommandResponse>
{
    public Guid NoteId { get; init; } = Guid.Empty;
}
