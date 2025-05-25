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
public class UpdatePromptCommandHandler : IRequestHandler<UpdatePromptCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePromptCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public UpdatePromptCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(UpdatePromptCommand request, CancellationToken cancellationToken)
    {
        var prompt = await _databaseContext.Prompts.FirstOrDefaultAsync(x => x.Id == request.PromptId);
        if (prompt == null)
        {
            throw new BusinessException("提示词不存在") { StatusCode = 404 };
        }

        prompt.Content = request.Content;
        prompt.Name = request.Name;
        prompt.Description = request.Description;
        prompt.Tags = string.Join(',', request.Tags);
        prompt.Type = request.PromptType.ToString();

        _databaseContext.Prompts.Update(prompt);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return EmptyCommandResponse.Default;
    }
}