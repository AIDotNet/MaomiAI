// <copyright file="CreateNoteCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Note.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace MaomiAI.Note.Handlers;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, IdResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateNoteCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="userContext"></param>
    public CreateNoteCommandHandler(DatabaseContext databaseContext, UserContext userContext)
    {
        _databaseContext = databaseContext;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public async Task<IdResponse> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        using TransactionScope transactionScope = new TransactionScope(
            scopeOption: TransactionScopeOption.Required,
            asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled,
            transactionOptions: new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
        var note = new NoteEntity
        {
            Title = request.Title,
            Summary = string.Empty,
            TitleEmoji = request.TitleEmoji ?? string.Empty,
            Content = request.Content ?? string.Empty,
        };

        if (request.ParentNoteId != null && request.ParentNoteId != Guid.Empty)
        {
            var parentNote = await _databaseContext.Notes
                .Where(x => x.Id == request.ParentNoteId && x.CreateUserId == _userContext.UserId)
                .Select(x => new { x.Id, x.CurrentPath })
                .FirstOrDefaultAsync(cancellationToken);

            if (parentNote == null)
            {
                throw new BusinessException("父级笔记不存在");
            }

            note.ParentId = parentNote.Id;
            note.ParentPath = parentNote.CurrentPath;
        }
        else
        {
            note.ParentId = Guid.Empty;
            note.ParentPath = "/root";
        }

        await _databaseContext.Notes.AddAsync(note, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        note.CurrentPath = $"{note.ParentPath}/{note.NoteId}";
        _databaseContext.Notes.Update(note);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        transactionScope.Complete();

        return new IdResponse { Id = note.Id };
    }
}
