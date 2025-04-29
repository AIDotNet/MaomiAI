// <copyright file="ITokenProvider.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace MaomiAI.User.Core.Services;

/// <summary>
/// 构建 token.
/// </summary>
public interface ITokenProvider
{
    /// <summary>
    /// 生成 JWT Access Token 和 Refresh Token.
    /// </summary>
    /// <param name="userContext"></param>
    /// <returns>token.</returns>
    (string AccessToken, string RefreshToken) GenerateTokens(UserContext userContext);

    /// <summary>
    /// 从 token 解析用户信息.
    /// </summary>
    /// <param name="token"></param>
    /// <returns>用户信息.</returns>
    Task<(UserContext UserContext, IReadOnlyDictionary<string, Claim> Claims)> ParseUserContextFromTokenAsync(string token);

    /// <summary>
    /// 验证token 是否有效，AccessToken 或 RefreshToken.
    /// </summary>
    /// <param name="token"></param>
    /// <returns>检验结果.</returns>
    Task<TokenValidationResult> ValidateTokenAsync(string token);
}