// <copyright file="QueryNoteCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Note.Queries.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Note.Queries;

public class QueryNoteCommandHandler : IRequestHandler<QueryNoteCommand, QueryNoteCommandResponse>
{
    public readonly DatabaseContext _databaseContext;
    public readonly UserContext _userContext;

    public QueryNoteCommandHandler(DatabaseContext databaseContext, UserContext userContext)
    {
        _databaseContext = databaseContext;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public async Task<QueryNoteCommandResponse> Handle(QueryNoteCommand request, CancellationToken cancellationToken)
    {
        var result = await _databaseContext.Notes
            .Where(x => x.CreateUserId == _userContext.UserId && x.Id == request.NoteId)
            .Select(x => new QueryNoteCommandResponse
            {
                Id = x.Id,
                Title = x.Title,
                TitleEmoji = x.TitleEmoji,
                Summary = x.Summary,
                Content = x.Content,
                ParentId = x.ParentId,
                ParentPath = x.ParentPath,
                CreateTime = x.CreateTime,
                IsShared = x.IsShared,
                NoteId = x.NoteId,
                UpdateTime = x.UpdateTime,
                CurrentPath = x.CurrentPath,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            throw new BusinessException("笔记不存在") { StatusCode = 404 };
        }

        return result;
    }
}