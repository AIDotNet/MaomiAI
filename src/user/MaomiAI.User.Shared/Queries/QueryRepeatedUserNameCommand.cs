// <copyright file="QueryRepeatedUserNameCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.User.Shared.Queries;

/// <summary>
/// 检查用户名是否重复.
/// </summary>
public class QueryRepeatedUserNameCommand : IRequest<Simple<bool>>
{
    /// <summary>
    /// 用户名.
    /// </summary>
    public string UserName { get; set; } = string.Empty;
}