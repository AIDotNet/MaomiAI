// <copyright file="IFileFactory.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using MaomiAI.Store.Enums;

namespace MaomiAI.Store.Services;

/// <summary>
/// 文件存储工厂接口.
/// </summary>
public interface IFileStoreFactory
{
    /// <summary>
    /// 创建文件存储实例.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    IFileStore Create(FileVisibility type);
}