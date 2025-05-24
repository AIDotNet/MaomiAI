// <copyright file="QueryNoteTreeCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Note.Queries;

public class QueryNoteTreeCommandHandler : IRequestHandler<QueryNoteTreeCommand, QueryNoteTreeCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly UserContext _userContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryNoteTreeCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="userContext"></param>
    public QueryNoteTreeCommandHandler(DatabaseContext databaseContext, UserContext userContext)
    {
        _databaseContext = databaseContext;
        _userContext = userContext;
    }

    /// <inheritdoc/>
    public async Task<QueryNoteTreeCommandResponse> Handle(QueryNoteTreeCommand request, CancellationToken cancellationToken)
    {
        var query = _databaseContext.Notes
            .Where(x => x.CreateUserId == _userContext.UserId);

        if (request.ParantId != null)
        {
            var parantPath = await _databaseContext.Notes
                .Where(x => x.Id == request.ParantId && x.CreateUserId == _userContext.UserId)
                .Select(x => x.CurrentPath)
                .FirstOrDefaultAsync(cancellationToken);

            if (parantPath == null)
            {
                throw new BusinessException("父级笔记不存在") { StatusCode = 404 };
            }

            query = query.Where(x => x.CurrentPath.StartsWith(parantPath));
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x => x.Title.Contains(request.Search));
        }

        var result = await query.Select(x => new NoteTreeItem
        {
            NoteId = x.Id,
            ParentId = x.ParentId,
            Title = x.Title,
            TitleEmoji = x.TitleEmoji,
            Summary = request.IncludeSummary ? x.Summary : string.Empty,
        })
            .ToListAsync();

        var exclude = result;
        var tree = new List<NoteTreeItem>();

        var rootNotes = result
            .Where(x => x.ParentId == default)
            .ToList();

        // 第一层
        tree.AddRange(rootNotes);

        foreach (var item in rootNotes)
        {
            exclude.Remove(item);
        }

        if (exclude.Count == 0)
        {
            return new QueryNoteTreeCommandResponse { Notes = tree };
        }

        var queue = new Queue<NoteTreeItem>(rootNotes);

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();

            var children = exclude
                .Where(x => x.ParentId == item.NoteId)
                .ToList();

            foreach (var childrenItem in children)
            {
                item.Children.Add(childrenItem);
                queue.Enqueue(childrenItem);
                exclude.Remove(childrenItem);
            }
        }

        return new QueryNoteTreeCommandResponse { Notes = tree };
    }
}