// <copyright file="CheckFileExistCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Queries.Response;
using MediatR;

namespace MaomiAI.Store.Queries;

/// <summary>
/// 检查文件是否存在
/// </summary>
public class CheckFileExistCommand : IRequest<CheckFileExistCommandResponse>
{
    public string MD5 { get; set; } = default!;
}
