// <copyright file="FileStoreHelper.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Team.Shared.Helpers;

public static class FileStoreHelper
{
    /// <summary>
    /// 生成文件 ObjectKey.
    /// </summary>
    /// <param name="md5"></param>
    /// <param name="fileName">文件名称.</param>
    /// <returns>ObjectKey.</returns>
    public static string GetObjectKey(string md5, string fileName)
    {
        var fileExtensions = Path.GetExtension(fileName);
        return $"{DateTimeOffset.Now.ToString("yyyyMMdd")}/{md5}.{fileExtensions}";
    }
}
