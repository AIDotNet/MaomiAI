// <copyright file="IFileStore.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

namespace MaomiAI.Store.Services;

public interface IPrivateFileStore : IFileStore { }
public interface IPublicFileStore : IFileStore { }


/// <summary>
/// 文件存储接口.
/// </summary>
public interface IFileStore
{
    /// <summary>
    /// 上传文件并返回文件地址.
    /// </summary>
    /// <param name="inputStream"></param>
    /// <param name="objectKey"></param>
    /// <returns>Task.</returns>
    Task UploadFileAsync(Stream inputStream, string objectKey);

    /// <summary>
    /// 批量获取文件地址，返回文件地址字典.
    /// </summary>
    /// <param name="objectKeys"></param>
    /// <param name="expiryDuration"></param>
    /// <returns>文件地址.</returns>
    Task<IReadOnlyDictionary<string, Uri>> GetFileUrlAsync(IEnumerable<string> objectKeys, TimeSpan expiryDuration);

    /// <summary>
    /// 判断文件是否存在.
    /// </summary>
    /// <param name="objectKey"></param>
    /// <returns>是否存在.</returns>
    Task<bool> FileExistsAsync(string objectKey);

    /// <summary>
    /// 文件是否存在.
    /// </summary>
    /// <param name="objectKeys"></param>
    /// <returns>是否存在.</returns>
    Task<IReadOnlyDictionary<string, bool>> FilesExistsAsync(IEnumerable<string> objectKeys);

    /// <summary>
    /// 获取文件大小.
    /// </summary>
    /// <param name="objectKey"></param>
    /// <returns>文件信息.</returns>
    Task<long> GetFileSizeAsync(string objectKey);

    /// <summary>
    /// 批量获取文件大小，返回文件大小字典.
    /// </summary>
    /// <param name="objectKeys"></param>
    /// <returns>文件信息.</returns>
    Task<IReadOnlyDictionary<string, long>> GetFileSizeAsync(IEnumerable<string> objectKeys);

    /// <summary>
    /// 批量删除文件.
    /// </summary>
    /// <param name="objectKeys"></param>
    /// <returns>Task.</returns>
    Task DeleteFilesAsync(IEnumerable<string> objectKeys);

    /// <summary>
    /// 生成预签名上传地址.
    /// </summary>
    /// <param name="fileObject"></param>
    /// <returns>预签名地址.</returns>
    Task<string> GeneratePreSignedUploadUrlAsync(FileObject fileObject);

    /// <summary>
    /// 下载文件.
    /// </summary>
    /// <param name="objectKey"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    Task DownloadAsync(string objectKey, string filePath);
}
