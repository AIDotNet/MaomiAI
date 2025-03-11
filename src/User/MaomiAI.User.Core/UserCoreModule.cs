// <copyright file="UserModule.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.Reflection;
using System.Text;

using Maomi;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MaomiAI.User.Core;

/// <summary>
/// 用户模块.
/// </summary>
public class UserCoreModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        // 注册中介者
        context.Services.AddScoped<IMediator, Mediator>();
    }

} 