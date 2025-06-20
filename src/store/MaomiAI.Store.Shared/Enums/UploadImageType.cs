﻿// <copyright file="UploadImageType.cs" company="MaomiAI">
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
    UserAvatar = 1,

    /// <summary>
    /// 团队中的.
    /// </summary>
    TeamAvatar = 2,

    /// <summary>
    /// 知识库的头像.
    /// </summary>
    WikiAvatar = 3,

    /// <summary>
    /// 知识库头像头像.
    /// </summary>
    PromptAvatar = 4,

    /// <summary>
    /// 笔记的图像.
    /// </summary>
    NoteImage = 5
}