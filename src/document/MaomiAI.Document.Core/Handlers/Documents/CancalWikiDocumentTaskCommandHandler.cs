﻿// <copyright file="CancalWikiDocumentTaskCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Document.Shared.Commands.Documents;
using MaomiAI.Document.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Handlers.Documents;

/// <summary>
/// 取消文档任务.
/// </summary>
public class CancalWikiDocumentTaskCommandHandler : IRequestHandler<CancalWikiDocumentTaskCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancalWikiDocumentTaskCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public CancalWikiDocumentTaskCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(CancalWikiDocumentTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _databaseContext.TeamWikiDocumentTasks
            .FirstOrDefaultAsync(x => x.DocumentId == request.DocumentId && x.Id == request.TaskId, cancellationToken);

        if (task == null || task.State > (int)FileEmbeddingState.Processing)
        {
            return EmptyCommandResponse.Default;
        }

        task.State = (int)FileEmbeddingState.Cancal;
        task.Message = "任务已取消";

        _databaseContext.TeamWikiDocumentTasks.Update(task);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return EmptyCommandResponse.Default;
    }
}
