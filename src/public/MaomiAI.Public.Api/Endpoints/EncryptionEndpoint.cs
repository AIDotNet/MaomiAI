// <copyright file="EncryptionEndpoint.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FastEndpoints;
using MaomiAI.Infra.Models;
using MaomiAI.Infra.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace MaomiAI.Public.Endpoints;

/// <summary>
/// 加密信息.
/// </summary>
[EndpointGroupName("public")]
[HttpPost($"{PublicApi.ApiPrefix}/encryption")]
[AllowAnonymous]
public class EncryptionEndpoint : Endpoint<Simple<string>, Simple<string>>
{
    private readonly IRsaProvider _rsaProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EncryptionEndpoint"/> class.
    /// </summary>
    /// <param name="rsaProvider"></param>
    public EncryptionEndpoint(IRsaProvider rsaProvider)
    {
        _rsaProvider = rsaProvider;
    }

    /// <inheritdoc/>
    public override Task<Simple<string>> ExecuteAsync(Simple<string> request, CancellationToken ct)
    {
        return Task.FromResult(new Simple<string>
        {
            Data = _rsaProvider.Encrypt(request.Data)
        });
    }
}
