// <copyright file="RefreshTokenCommandHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Database;
using MaomiAI.Infra.Defaults;
using MaomiAI.User.Core.Services;
using MaomiAI.User.Shared.Commands;
using MaomiAI.User.Shared.Commands.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MaomiAI.User.Core.Handlers;

/// <summary>
/// 刷新 token.
/// </summary>
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginCommandResponse>
{
    private readonly ITokenProvider _tokenProvider;
    private readonly DatabaseContext _dbContext;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenCommandHandler"/> class.
    /// </summary>
    /// <param name="tokenProvider"></param>
    /// <param name="dbContext"></param>
    /// <param name="logger"></param>
    public RefreshTokenCommandHandler(ITokenProvider tokenProvider, DatabaseContext dbContext, ILogger<RefreshTokenCommandHandler> logger)
    {
        _tokenProvider = tokenProvider;
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<LoginCommandResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenValidationResult = await _tokenProvider.ValidateTokenAsync(request.RefreshToken);

        if (!tokenValidationResult.IsValid)
        {
            throw new BusinessException("token 验证失败.") { StatusCode = 401 };
        }

        var (refreshTokenUserContext, claims) = await _tokenProvider.ParseUserContextFromTokenAsync(request.RefreshToken);

        // 如果不是 refresh_token，禁止刷新 token
        if (!claims.TryGetValue("token_type", out var tokenType) || string.IsNullOrEmpty(tokenType.Value) || !"refresh".Equals(tokenType.Value, StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessException("非 refresh_token.") { StatusCode = 401 };
        }

        var user = await _dbContext.Users.Where(x => x.Id == refreshTokenUserContext.UserId)
                      .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new BusinessException("用户不存在") { StatusCode = 401 };
        }

        if (!user.IsEnable)
        {
            throw new BusinessException("用户已被禁用") { StatusCode = 401 };
        }

        var userContext = new DefaultUserContext
        {
            UserId = user.Id,
            UserName = user.UserName,
            NickName = user.NickName,
            Email = user.Email
        };

        var (accessToken, refreshToken) = _tokenProvider.GenerateTokens(userContext);

        var result = new LoginCommandResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserId = user.Id,
            UserName = user.UserName,
            ExpiresIn = DateTimeOffset.Now.AddMinutes(30).ToUnixTimeMilliseconds()
        };

        _logger.LogInformation("User refresh token.{@Message}", new { user.Id, user.UserName, user.NickName });

        return result;
    }
}
