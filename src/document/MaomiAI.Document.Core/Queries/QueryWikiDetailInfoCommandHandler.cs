// <copyright file="QueryWikiSimpleInfoCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Document.Shared.Queries;
using MaomiAI.Document.Shared.Queries.Response;
using MaomiAI.Infra;
using MaomiAI.Store.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaomiAI.Document.Core.Queries;

public class QueryWikiDetailInfoCommandHandler : IRequestHandler<QueryWikiDetailInfoCommand, QueryWikiDetailInfoResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiDetailInfoCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="mediator"></param>
    /// <param name="systemOptions"></param>
    public QueryWikiDetailInfoCommandHandler(DatabaseContext databaseContext, IMediator mediator, SystemOptions systemOptions)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
        _systemOptions = systemOptions;
    }

    /// <inheritdoc/>
    public async Task<QueryWikiDetailInfoResponse> Handle(QueryWikiDetailInfoCommand request, CancellationToken cancellationToken)
    {
        var wiki = await _databaseContext.TeamWikis.Where(x => x.Id == request.WikiId)
            .Select(x => new QueryWikiDetailInfoResponse
            {
                WikiId = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsPublic = x.IsPublic,
                AvatarUrl = x.AvatarPath,
                Markdown = x.Markdown
            }).FirstOrDefaultAsync();

        if (wiki == null)
        {
            throw new BusinessException("知识库不存在") { StatusCode = 404 };
        }

        var avatarUrl = string.Empty;
        if (!string.IsNullOrEmpty(wiki.AvatarUrl))
        {
            var fileUrls = await _mediator.Send(new QueryPublicFileUrlFromPathCommand { ObjectKeys = new List<string>() { wiki.AvatarUrl } });
            avatarUrl = fileUrls.Urls.First().Value!;
        }
        else
        {
            avatarUrl = new Uri(new Uri(_systemOptions.Server), "default/avatar.png").ToString();
        }

        return wiki;
    }
}
