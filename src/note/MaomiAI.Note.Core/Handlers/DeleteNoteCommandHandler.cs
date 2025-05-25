// <copyright file="DeleteNoteCommandHandler.cs" company="MaomiAI">
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

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly UserContext _userContext;

    public DeleteNoteCommandHandler(DatabaseContext databaseContext, UserContext userContext)
    {
        _databaseContext = databaseContext;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        var note = await _databaseContext.Notes.Where(x => x.CreateUserId == _userContext.UserId && x.Id == request.NoteId).Select(x => new
        {
            x.Id,
            x.CurrentPath,
        }).FirstOrDefaultAsync();

        if (note == null)
        {
            throw new BusinessException("笔记不存在") { StatusCode = 404 };
        }

        await _databaseContext.Notes.Where(x => x.CurrentPath.StartsWith(note.CurrentPath))
            .ExecuteUpdateAsync(x => x.SetProperty(a => a.IsDeleted, true));

        // 删除后后台清理笔记文件等

        return EmptyCommandResponse.Default;
    }
}
