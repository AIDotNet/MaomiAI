// <copyright file="QueryUserInfoCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Infra;
using MaomiAI.Store.Queries;
using MaomiAI.Team.Shared.Helpers;
using MaomiAI.User.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.User.Api.Handlers;

/// <summary>
/// 处理查询用户信息的命令.
/// </summary>
public class QueryUserInfoCommandHandler : IRequestHandler<QueryUserInfoCommand, QueryUserInfoCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly SystemOptions _systemOptions;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryUserInfoCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="systemOptions"></param>
    /// <param name="mediator"></param>
    public QueryUserInfoCommandHandler(DatabaseContext databaseContext, SystemOptions systemOptions, IMediator mediator)
    {
        _databaseContext = databaseContext;
        _systemOptions = systemOptions;
        _mediator = mediator;
    }

    /// <inheritdoc/>
    public async Task<QueryUserInfoCommandResponse> Handle(QueryUserInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new BusinessException("未找到用户.") { StatusCode = 404 };
        }

        var avatarUrl = string.Empty;
        if (user.AvatarId != default)
        {
            var fileUrls = await _mediator.Send(new QueryPublicFileUrlFromPathCommand { ObjectKeys = new List<string>() { user.AvatarPath } });
            avatarUrl = fileUrls.Urls.First().Value!;
        }
        else
        {
            avatarUrl = new Uri(new Uri(_systemOptions.Server), "default/avatar.png").ToString();
        }

        return new QueryUserInfoCommandResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            NickName = user.NickName,
            Avatar = avatarUrl
        };
    }
}
