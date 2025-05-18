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
using System.Data.Entity;

namespace MaomiAI.Document.Core.Queries;

/// <summary>
/// 查询知识库简单信息.
/// </summary>
public class QueryWikiSimpleInfoCommandHandler : IRequestHandler<QueryWikiSimpleInfoCommand, QueryWikiSimpleInfoResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IMediator _mediator;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryWikiSimpleInfoCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="mediator"></param>
    /// <param name="systemOptions"></param>
    public QueryWikiSimpleInfoCommandHandler(DatabaseContext databaseContext, IMediator mediator, SystemOptions systemOptions)
    {
        _databaseContext = databaseContext;
        _mediator = mediator;
        _systemOptions = systemOptions;
    }

    /// <inheritdoc/>
    public async Task<QueryWikiSimpleInfoResponse> Handle(QueryWikiSimpleInfoCommand request, CancellationToken cancellationToken)
    {
        var wiki = await _databaseContext.TeamWikis.Where(x => x.Id == request.WikiId)
            .Select(x => new QueryWikiSimpleInfoResponse
            {
                WikiId = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsPublic = x.IsPublic,
                AvatarUrl = x.AvatarPath
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
