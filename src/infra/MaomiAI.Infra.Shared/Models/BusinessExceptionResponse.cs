﻿// <copyright file="ErrorResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using FluentValidation.Results;

namespace MaomiAI.Infra.Models;

/// <summary>
/// 错误响应模型.
/// </summary>
public class BusinessExceptionResponse
{
    /// <summary>
    /// 请求上下文 ID.
    /// </summary>
    public string RequestId { get; init; } = string.Empty;

    /// <summary>
    /// 错误码.
    /// </summary>
    public int Code { get; init; } = 500;

    /// <summary>
    /// 错误消息.
    /// </summary>
    public string Detail { get; init; } = string.Empty;

    /// <summary>
    /// 具体错误列表.
    /// </summary>
    public IReadOnlyCollection<BusinessExceptionError>? Errors { get; init; }

    /// <summary>
    /// 扩展.
    /// </summary>
    public IReadOnlyDictionary<string, object?>? Extensions { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessExceptionResponse"/> class.
    /// </summary>
    public BusinessExceptionResponse()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessExceptionResponse"/> class.
    /// </summary>
    /// <param name="failures"></param>
    /// <param name="statusCode"></param>
    public BusinessExceptionResponse(IReadOnlyList<ValidationFailure> failures, int statusCode = 400)
    {
        // Microsoft.AspNetCore.Mvc.ValidationProblemDetails
        Code = statusCode;
        Errors = failures.GroupBy(f => f.PropertyName).Select(e => new BusinessExceptionError
        {
            Name = e.Key,
            Errors = e.Select(m => m.ErrorMessage).ToArray()
        }).ToArray();
    }
}
