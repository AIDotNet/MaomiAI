// <copyright file="UploadImageType.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Store.Enums;

/// <summary>
/// 上传的图像类型.
/// </summary>
public enum UploadImageType
{
    /// <summary>
    /// 图片图片.
    /// </summary>
    None = 0,

    /// <summary>
    /// 头像.
    /// </summary>
    Avatar = 1,

    /// <summary>
    /// 团队中的.
    /// </summary>
    Team = 2,

    /// <summary>
    /// 知识库中的
    /// </summary>
    Document = 3,
}