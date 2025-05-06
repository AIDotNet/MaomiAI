// <copyright file="CheckUserStateCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.User.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaomiAI.User.Core.Handlers;

/// <summary>
/// <inheritdoc cref="CheckUserStateCommand"/>
/// </summary>
public class CheckUserStateCommandHandler : IRequestHandler<CheckUserStateCommand, EmptyCommandResponse>
{
    private readonly DatabaseContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckUserStateCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public CheckUserStateCommandHandler(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task<EmptyCommandResponse> Handle(CheckUserStateCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new BusinessException("用户不存在") { StatusCode = 400 };
        }

        if (!user.IsEnable)
        {
            throw new BusinessException("用户已被禁用") { StatusCode = 400 };
        }

        return EmptyCommandResponse.Default;
    }
}
