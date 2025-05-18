// <copyright file="DeleteWikiCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Document.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.Document.Core.Handlers;

/// <summary>
/// 删除 wiki.
/// </summary>
public class DeleteWikiCommandHandler : IRequestHandler<DeleteWikiCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteWikiCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public DeleteWikiCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(DeleteWikiCommand request, CancellationToken cancellationToken)
    {
        var wiki = await _databaseContext.TeamWikis.Where(x => x.Id == request.WikiId).FirstOrDefaultAsync();
        if (wiki == null)
        {
            throw new BusinessException("知识库不存在") { StatusCode = 404 };
        }

        _databaseContext.Remove(wiki);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        // todo: 发出命令清理内容

        return EmptyCommandResponse.Default;
    }
}