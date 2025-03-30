// <copyright file="ErrorMessageResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Dtos;

/// <summary>
/// 错误消息响应.
/// </summary>
public class ErrorMessageResponse
{
    /// <summary>
    /// 错误代码.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 错误消息.
    /// </summary>
    public string Message { get; set; } = default!;
}
