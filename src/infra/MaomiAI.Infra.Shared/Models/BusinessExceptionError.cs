// <copyright file="ErrorResponse.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Infra.Models;

/// <summary>
/// 错误信息.
/// </summary>
public class BusinessExceptionError
{
    /// <summary>
    /// 名称.
    /// </summary>
    public string Name { get; init; } = default!;

    /// <summary>
    /// 错误信息列表.
    /// </summary>
    public IReadOnlyCollection<string> Errors { get; init; } = default!;
}