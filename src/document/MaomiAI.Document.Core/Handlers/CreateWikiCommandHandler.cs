// <copyright file="CreateWikiCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Database.Entities;
using MaomiAI.Document.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Handlers;

/// <summary>
/// 创建知识库.
/// </summary>
public class CreateWikiCommandHandler : IRequestHandler<CreateWikiCommand, IdResponse>
{
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateWikiCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public CreateWikiCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<IdResponse> Handle(CreateWikiCommand request, CancellationToken cancellationToken)
    {
        var exist = await _databaseContext.TeamWikis.AnyAsync(x => x.TeamId == request.TeamId && x.Name == request.Name, cancellationToken);
        if (exist)
        {
            throw new BusinessException("已存在同名知识库") { StatusCode = 409 };
        }

        var entity = new TeamWikiEntity
        {
            Id = Guid.NewGuid(),
            TeamId = request.TeamId,
            Name = request.Name,
            Description = request.Description,
        };

        await _databaseContext.TeamWikis.AddAsync(entity, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return new IdResponse
        {
            Id = entity.Id
        };
    }
}
