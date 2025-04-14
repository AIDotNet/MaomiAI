// <copyright file="MVCExceptionFilter.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

#pragma warning disable SA1118 // Parameter should not span multiple lines

using Maomi;
using Maomi.AI.Exceptions;
using MaomiAI.Infra.Diagnostics;
using MaomiAI.Infra.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace MaomiAI.Filters;

/// <summary>
/// MVC异常处理.
/// </summary>
[InjectOn(Scheme = InjectScheme.None, Own = true)]
public class MVCExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<MVCExceptionFilter> _logger;
    private readonly IStringLocalizer _stringLocalizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="MVCExceptionFilter"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="stringLocalizer"></param>
    public MVCExceptionFilter(ILogger<MVCExceptionFilter> logger, IStringLocalizer stringLocalizer)
    {
        _logger = logger;
        _stringLocalizer = stringLocalizer;
    }

    /// <inheritdoc/>
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (context.ExceptionHandled)
        {
            return;
        }
        // todo: httplogging 应该有，应该不需要自己写了，需要验证一下
        var action = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;

        using var activity = ActivityHelper.ActivitySource.StartActivity(
            "EndpointsException",
            ActivityKind.Server,
            parentContext: default,
            tags: new ActivityTagsCollection
            {
                { "RequestId", context.HttpContext.TraceIdentifier },
                { "ControllerName", action?.ControllerName },
                { "ActionName", action?.ActionName },
                { "ExceptionType", context.Exception.GetType().FullName },
                { "ExceptionMessage", context.Exception.Message },
                { "ExceptionDetail", context.Exception }
            });

        _logger.LogError(
            context.Exception,
            """
            RequestId: {RequestId},
            ControllerName: {ControllerName},
            ActionName: {ActionName},
            ExceptionType: {ExceptionType},
            ExceptionMessage: {ExceptionMessage},
            ExceptionDetail: {ExceptionDetail},
            """,
            context.HttpContext.TraceIdentifier,
            action?.ControllerName,
            action?.ActionName,
            context.Exception.GetType().Name,
            context.Exception.Message,
            context.Exception.ToString());

        var message = string.Empty;
        var messageDetail = string.Empty;

        if (context.Exception is BusinessException businessException)
        {
            message = businessException.Message;
            messageDetail = businessException.ToString();
        }
        else
        {
#if DEBUG
            message = context.Exception.Message;
            messageDetail = context.Exception.ToString();
#else
        Message = "Internal server error",
#endif
        }

        var response = new ErrorResponse()
        {
            Code = 500,
            RequestId = context.HttpContext.TraceIdentifier,
            Message = message,
            Detail = messageDetail,
        };

        context.Result = new ObjectResult(response)
        {
            StatusCode = 500,
        };

        context.ExceptionHandled = true;

        await Task.CompletedTask;
    }
}