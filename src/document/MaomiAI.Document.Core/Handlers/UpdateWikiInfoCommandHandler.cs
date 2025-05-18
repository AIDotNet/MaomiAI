// <copyright file="UpdateWikiInfoCommandHandler.cs" company="MaomiAI">
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
/// 更新知识库信息.
/// </summary>
public class UpdateWikiInfoCommandHandler : IRequestHandler<UpdateWikiInfoCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateWikiInfoCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    public UpdateWikiInfoCommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(UpdateWikiInfoCommand request, CancellationToken cancellationToken)
    {
        // 获取知识库实体
        var wiki = await _databaseContext.TeamWikis.FirstOrDefaultAsync(x => x.Id == request.WikiId, cancellationToken);
        if (wiki == null)
        {
            throw new BusinessException("知识库不存在.");
        }

        // 更新实体信息
        wiki.Name = request.Name;
        wiki.Description = request.Description;
        wiki.Markdown = request.Markdown;
        wiki.IsPublic = request.IsPublic;

        // 保存更改
        _databaseContext.Update(wiki);
        await _databaseContext.SaveChangesAsync(cancellationToken);
        return EmptyCommandResponse.Default;
    }
}
