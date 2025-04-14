// <copyright file="CustomGlobalExceptionHandler.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Maomi.AI.Exceptions;
using MaomiAI.Infra.Diagnostics;
using MaomiAI.Infra.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace MaomiAI.Filters;

/// <summary>
/// 全局中间件未知异常拦截.<br />
/// MVC 异常拦截 <see cref="MVCExceptionFilter"/>.
/// </summary>
public class CustomGlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomGlobalExceptionHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomGlobalExceptionHandler"/> class.
    /// </summary>
    /// <param name="logger"></param>
    public CustomGlobalExceptionHandler(ILogger<CustomGlobalExceptionHandler> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc/>
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var message = string.Empty;
        var messageDetail = string.Empty;

        if (exception is BusinessException businessException)
        {
            message = businessException.Message;
            messageDetail = businessException.ToString();
        }
        else
        {
#if DEBUG
            message = exception.Message;
            messageDetail = exception.ToString();
#else
        Message = "Internal server error",
#endif
        }

        var response = new ErrorResponse()
        {
            Code = 500,
            RequestId = httpContext.TraceIdentifier,
            Message = message,
            Detail = messageDetail,
        };

        httpContext.Response.WriteAsJsonAsync(
            response,
            cancellationToken: cancellationToken);

        httpContext.Response.StatusCode = 500;

        return ValueTask.FromResult(true);
    }
}