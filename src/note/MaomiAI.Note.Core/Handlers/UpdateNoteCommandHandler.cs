// <copyright file="UpdateNoteTitleCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Note.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Note.Handlers;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateNoteCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="userContext"></param>
    public UpdateNoteCommandHandler(DatabaseContext databaseContext, UserContext userContext)
    {
        _databaseContext = databaseContext;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _databaseContext.Notes
            .Where(x => x.Id == request.NoteId && x.CreateUserId == _userContext.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (note == null)
        {
            throw new BusinessException("笔记不存在") { StatusCode = 404 };
        }

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            note.Title = request.Title;
        }

        if (!string.IsNullOrWhiteSpace(request.TitleEmoji))
        {
            note.TitleEmoji = request.TitleEmoji;
        }

        if (!string.IsNullOrWhiteSpace(request.Summary))
        {
            note.Summary = request.Summary;
        }

        if (!string.IsNullOrWhiteSpace(request.Content))
        {
            note.Content = request.Content;
        }

        if (request.IsShared != null)
        {
            note.IsShared = request.IsShared.Value;
        }

        _databaseContext.Notes.Update(note);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return EmptyCommandResponse.Default;
    }
}
