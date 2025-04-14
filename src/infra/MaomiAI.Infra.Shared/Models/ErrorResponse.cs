// <copyright file="ErrorResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra.Models;

/// <summary>
/// 错误响应模型.
/// </summary>
public class ErrorResponse
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
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// 错误详情.
    /// </summary>
    public string Detail { get; init; } = string.Empty;
}
