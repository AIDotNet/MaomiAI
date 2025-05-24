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

public class CreatePromptCommandHandler : IRequestHandler<CreatePromptCommand, IdResponse>
{
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePromptCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public CreatePromptCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IdResponse> Handle(CreatePromptCommand request, CancellationToken cancellationToken)
    {
        var exist = await _databaseContext.Prompts.AnyAsync(x => x.TeamId == request.TeamId && x.Name == request.Name);
        if (exist)
        {
            throw new BusinessException("提示词名称已存在") { StatusCode = 409 };
        }

        var prompt = new PromptEntity
        {
            Name = request.Name,
            Description = request.Description,
            Content = request.Content,
            Tags = string.Join(',', request.Tags),
            TeamId = request.TeamId,
            AvatarPath = string.Empty,
        };

        await _databaseContext.Prompts.AddAsync(prompt, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);
        return new IdResponse
        {
            Id = prompt.Id
        };
    }
}
