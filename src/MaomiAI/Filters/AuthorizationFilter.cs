// <copyright file="AuthorizationFilter.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MaomiAI.Filters
{
    /// <summary>
    /// 全局授权过滤器.
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly ILogger<AuthorizationFilter> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationFilter"/> class.
        /// </summary>
        /// <param name="logger">日志记录器.</param>
        public AuthorizationFilter(ILogger<AuthorizationFilter> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 检查是否有AllowAnonymous特性
            if (HasAllowAnonymousAttribute(context))
            {
                return;
            }

            // 检查用户是否已认证
            if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
            {
                _logger.LogWarning("未授权访问: {Path}", context.HttpContext.Request.Path);
                context.Result = new UnauthorizedResult();
            }
        }

        private static bool HasAllowAnonymousAttribute(AuthorizationFilterContext context)
        {
            // 检查控制器或操作是否有AllowAnonymous特性
            if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
            {
                // 检查操作是否有AllowAnonymous特性
                if (actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
                {
                    return true;
                }

                // 检查控制器是否有AllowAnonymous特性
                if (actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true)
                    .Any())
                {
                    return true;
                }
            }

            return false;
        }
    }
}