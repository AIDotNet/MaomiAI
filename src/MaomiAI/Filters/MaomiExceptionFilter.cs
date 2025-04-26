﻿// <copyright file="MVCExceptionFilter.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

#pragma warning disable SA1118 // Parameter should not span multiple lines

using Maomi;
using Maomi.AI.Exceptions;
using MaomiAI.Infra.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace MaomiAI.Filters;

/// <summary>
/// MVC异常处理.
/// </summary>
[InjectOn(Own = true)]
public class MaomiExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<MaomiExceptionFilter> _logger;
    private readonly IStringLocalizer _stringLocalizer;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaomiExceptionFilter"/> class.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="stringLocalizer"></param>
    public MaomiExceptionFilter(ILogger<MaomiExceptionFilter> logger, IStringLocalizer stringLocalizer)
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

        if (context.Exception is BusinessException businessException)
        {
            HandleBusinessException(context, businessException);
        }
        else
        {
            ProcessUnhandledException(context);
        }

        context.ExceptionHandled = true;

        await Task.CompletedTask;
    }

    private static void ProcessUnhandledException(ExceptionContext context)
    {
        var message = string.Empty;
        var messageDetail = string.Empty;

#if DEBUG
        message = context.Exception.Message;
        messageDetail = context.Exception.ToString();
#else
        Message = "Internal server error",
#endif

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
    }

    private static void HandleBusinessException(ExceptionContext context, BusinessException businessException)
    {
        var message = string.Empty;
        var messageDetail = string.Empty;

        message = businessException.Message;

#if DEBUG
        messageDetail = businessException.ToString();
#endif

        var response = new ErrorResponse()
        {
            Code = businessException.ErrorCode,
            RequestId = context.HttpContext.TraceIdentifier,
            Message = message,
            Detail = messageDetail,
        };

        context.Result = new ObjectResult(response)
        {
            StatusCode = businessException.StatusCode,
        };
    }
}