// <copyright file="IFileDownClient.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using System.ComponentModel.DataAnnotations;
using Refit;

namespace MaomiAI.Store.Clients;

public interface IFileDownClient
{
    public HttpClient Client { get; }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="fileUrl">文件的完整URL地址</param>
    /// <returns>文件流</returns>
    [Get("")]
    Task<Stream> DownloadFileAsync([AliasAs("")][Url] string fileUrl);
}
