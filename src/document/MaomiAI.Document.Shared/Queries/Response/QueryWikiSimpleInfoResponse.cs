// <copyright file="QueryWikiSimpleInfoCommand.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MediatR;

namespace MaomiAI.Document.Shared.Queries.Response;

public class QueryWikiSimpleInfoResponse
{
    /// <summary>
    /// 知识库 id.
    /// </summary>
    public Guid WikiId { get; init; }

    /// <summary>
    /// 知识库名称.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 团队头像.
    /// </summary>
    public Guid AvatarId { get; set; }

    /// <summary>
    /// 团队头像路径.
    /// </summary>
    public string AvatarPath { get; set; }

    /// <summary>
    /// 知识库描述.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 公开使用，所有人不需要加入团队即可使用此知识库.
    /// </summary>
    public bool IsPublic { get; set; }
}
