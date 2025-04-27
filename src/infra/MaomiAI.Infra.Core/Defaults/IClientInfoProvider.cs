// <copyright file="IClientInfoProvider.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi;
using Maomi.AI.Exceptions;
using MaomiAI.Infra.Models;
using MaomiAI.Infra.Services;
using Microsoft.AspNetCore.Http;

namespace MaomiAI.Infra.Defaults;

[InjectOnScoped]
public class ClientInfoProvider : IClientInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Lazy<ClientInfo> _clientInfo;

    public ClientInfoProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _clientInfo = new Lazy<ClientInfo>(() =>
         {
             return ParseClientInfo();
         });
    }

    public ClientInfo GetClientInfo() => _clientInfo.Value;

    private ClientInfo ParseClientInfo()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            throw new BusinessException("HttpContext is not available.");
        }

        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";

        return new ClientInfo
        {
            IP = ip,
            UserAgent = userAgent
        };
    }
}
