// <copyright file="CreatePromptCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Infra.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Prompt.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Prompt.Core.Handlers;

public class PublishPromptCommandHandler : IRequestHandler<PublishPromptCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishPromptCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public PublishPromptCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<EmptyCommandResponse> Handle(PublishPromptCommand request, CancellationToken cancellationToken)
    {
        var prompt = await _databaseContext.Prompts.FirstOrDefaultAsync(x => x.Id == request.PromptId);
        if (prompt == null)
        {
            throw new BusinessException("提示词不存在") { StatusCode = 404 };
        }

        prompt.IsPublic = request.IsPublic;
        _databaseContext.Prompts.Update(prompt);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return EmptyCommandResponse.Default;
    }
}
