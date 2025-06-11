// <copyright file="ImportMcpServerCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Infra.Models;
using MaomiAI.Store.Enums;
using MediatR;

namespace MaomiAI.Plugin.Shared.Commands;

/// <summary>
/// 预上传 openapi 文件，支持 json、yaml.
/// </summary>
public class ComplateOpenApiFileCommand : IRequest<IdResponse>
{
    /// <summary>
    /// 团队 id.
    /// </summary>
    public Guid TeamId { get; init; }

    /// <summary>
    /// 上传的 id.
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// 分组名称.
    /// </summary>
    public string Name { get; init; } = default!;

}
