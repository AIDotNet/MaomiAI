// <copyright file="QueryUserInfoCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Infra;
using MaomiAI.Team.Shared.Helpers;
using MaomiAI.User.Shared.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.User.Api.Handlers;

/// <summary>
/// 处理查询用户信息的命令.
/// </summary>
public class QueryUserInfoCommandHandler : IRequestHandler<QueryUserInfoCommand, QueryUserInfoResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryUserInfoCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseContext"></param>
    /// <param name="systemOptions"></param>
    public QueryUserInfoCommandHandler(DatabaseContext databaseContext, SystemOptions systemOptions)
    {
        _databaseContext = databaseContext;
        _systemOptions = systemOptions;
    }

    /// <inheritdoc/>
    public async Task<QueryUserInfoResponse> Handle(QueryUserInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new BusinessException("未找到用户.");
        }

        return new QueryUserInfoResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            NickName = user.NickName,
            Avatar = FileStoreHelper.CombineUrl(_systemOptions.PublicStore.Endpoint, user.AvatarPath)
        };
    }
}
