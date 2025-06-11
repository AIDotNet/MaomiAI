// <copyright file="MoveNoteParentCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

#pragma warning disable CA1307 // 为了清晰起见，请指定 StringComparison

using MaomiAI.Database;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Note.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace MaomiAI.Note.Handlers;

public class MoveNoteParentCommandHandler : IRequestHandler<MoveNoteParentCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly UserContext _userContext;

    public async Task<EmptyCommandResponse> Handle(MoveNoteParentCommand request, CancellationToken cancellationToken)
    {
        // old
        var oldParentPath = await _databaseContext.Notes
            .Where(x => x.CreateUserId == _userContext.UserId && x.Id == _databaseContext.Notes.Where(a => a.Id == request.NoteId).Select(a => a.ParentId).First())
            .Select(x => new
            {
                x.NoteId,
                x.CurrentPath,
            })
            .FirstOrDefaultAsync(cancellationToken);

        // new
        var newParentPath = await _databaseContext.Notes
            .Where(x => x.CreateUserId == _userContext.UserId && x.Id == request.ParentId)
            .Select(x => new
            {
                x.Id,
                x.CurrentPath,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (oldParentPath == null || newParentPath == null)
        {
            throw new BusinessException("笔记不存在") { StatusCode = 404 };
        }

        using TransactionScope transactionScope = new TransactionScope(
            scopeOption: TransactionScopeOption.Required,
            asyncFlowOption: TransactionScopeAsyncFlowOption.Enabled,
            transactionOptions: new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead });

        await _databaseContext.Notes
            .Where(x => x.Id == request.NoteId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(a => a.ParentId, newParentPath.Id)
                .SetProperty(a => a.ParentPath, newParentPath.CurrentPath)
                .SetProperty(a => a.CurrentPath, a => newParentPath.CurrentPath + "/" + a.Id.ToString()));

        await _databaseContext.Notes
            .Where(x => x.CurrentPath.StartsWith(oldParentPath.CurrentPath))
            .ExecuteUpdateAsync(x => x.SetProperty(a => a.ParentPath, a => a.ParentPath.Replace(oldParentPath.CurrentPath, newParentPath.CurrentPath))
            .SetProperty(x => x.CurrentPath, a => a.CurrentPath.Replace(oldParentPath.CurrentPath, newParentPath.CurrentPath)));

        transactionScope.Complete();

        return EmptyCommandResponse.Default;
    }
}
