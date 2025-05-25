// <copyright file="DeleteAiModelCommandHanler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.AiModel.Shared.Commands;
using MaomiAI.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.AiModel.Core.Handlers;

public class DeleteAiModelCommandHanler : IRequestHandler<DeleteAiModelCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteAiModelCommandHanler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public DeleteAiModelCommandHanler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(DeleteAiModelCommand request, CancellationToken cancellationToken)
    {
        var aiModel = await _databaseContext.TeamAiModels
            .FirstOrDefaultAsync(x => x.TeamId == request.TeamId && x.Id == request.AiModelId, cancellationToken);

        if (aiModel != null)
        {
            _databaseContext.TeamAiModels.Remove(aiModel);
            await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        return EmptyCommandResponse.Default;
    }
}
