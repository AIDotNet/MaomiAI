// <copyright file="S3Store.cs" company="MaomiAI">
// Copyright (c) MaomiAI. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github link: https://github.com/AIDotNet/MaomiAI
// </copyright>

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MaomiAI.Infra;
using System.Diagnostics;
using System.Net;

namespace MaomiAI.Store.Services;

/// <summary>
/// S3 存储对接.
/// </summary>
public class S3Store : IFileStore, IDisposable
{
    private readonly SystemStoreOption _storeOption;
    private readonly AmazonS3Client _s3Client;
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="S3Store"/> class.
    /// </summary>
    /// <param name="storeOption"></param>
    public S3Store(SystemStoreOption storeOption)
    {
        _storeOption = storeOption;

        _s3Client = new AmazonS3Client(_storeOption.AccessKeyId, _storeOption.AccessKeySecret, new AmazonS3Config
        {
            ServiceURL = _storeOption.Endpoint,
            ForcePathStyle = storeOption.ForcePathStyle,
            UseHttp = true
        });
    }

    /// <inheritdoc/>
    public async Task UploadFileAsync(Stream inputStream, string objectKey)
    {
        using TransferUtility? fileTransferUtility = new(_s3Client);
        await fileTransferUtility.UploadAsync(inputStream, _storeOption.Bucket, objectKey);
    }

    /// <inheritdoc/>
    public async Task<string> GeneratePreSignedUploadUrlAsync(FileObject fileObject)
    {
        GetPreSignedUrlRequest request = new()
        {
            BucketName = _storeOption.Bucket,
            Key = fileObject.ObjectKey,
            Expires = DateTime.UtcNow.Add(fileObject.ExpiryDuration),
            Verb = HttpVerb.PUT,
        };

        // 限制上传的文件类型.
        if (!string.IsNullOrWhiteSpace(fileObject.ContentType))
        {
            request.ContentType = fileObject.ContentType;
        }

        // 可以在对象 metadata 中添加最大的文件大小信息
        //request.Headers["x-amz-meta-max-file-size"] = fileObject.MaxFileSize.ToString();

        string url = await _s3Client.GetPreSignedURLAsync(request);
        return url;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyDictionary<string, Uri>> GetFileUrlAsync(IEnumerable<string> objectKeys, TimeSpan expiryDuration)
    {
        IEnumerable<Task<KeyValuePair<string, Uri>>>? tasks = objectKeys.Select(async key =>
        {
            GetPreSignedUrlRequest? request = new()
            {
                BucketName = _storeOption.Bucket,
                Key = key,
                Expires = DateTime.UtcNow.Add(expiryDuration)
            };
            string? url = await _s3Client.GetPreSignedURLAsync(request);
            return new KeyValuePair<string, Uri>(key, new Uri(url));
        });

        KeyValuePair<string, Uri>[]? urls = await Task.WhenAll(tasks);

        return urls.ToDictionary();
    }

    /// <inheritdoc/>
    public async Task<bool> FileExistsAsync(string objectKey)
    {
        GetObjectMetadataRequest? request = new()
        {
            BucketName = _storeOption.Bucket,
            Key = objectKey
        };
        try
        {
            await _s3Client.GetObjectMetadataAsync(request);
            return true;
        }
        catch (AmazonS3Exception e) when (e.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyDictionary<string, bool>> FilesExistsAsync(IEnumerable<string> objectKeys)
    {
        IEnumerable<Task<KeyValuePair<string, bool>>>? tasks = objectKeys.Select(async key =>
        {
            GetObjectMetadataRequest? request = new()
            {
                BucketName = _storeOption.Bucket,
                Key = key
            };
            try
            {
                await _s3Client.GetObjectMetadataAsync(request);
                return new KeyValuePair<string, bool>(key, true);
            }
            catch (AmazonS3Exception e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return new KeyValuePair<string, bool>(key, false);
            }
        });

        var results = await Task.WhenAll(tasks);
        return results.ToDictionary();
    }

    /// <inheritdoc/>
    public async Task<long> GetFileSizeAsync(string objectKey)
    {
        GetObjectMetadataRequest? request = new()
        {
            BucketName = _storeOption.Bucket,
            Key = objectKey
        };

        try
        {
            var response = await _s3Client.GetObjectMetadataAsync(request);

            return response.ContentLength;
        }
        catch (AmazonS3Exception ex)
        {
            Debug.WriteLine(ex);
            return 0;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            return 0;
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyDictionary<string, long>> GetFileSizeAsync(IEnumerable<string> objectKeys)
    {
        IEnumerable<Task<KeyValuePair<string, long>>>? tasks = objectKeys.Select(async key =>
        {
            GetObjectMetadataRequest? request = new()
            {
                BucketName = _storeOption.Bucket,
                Key = key
            };
            GetObjectMetadataResponse? response = await _s3Client.GetObjectMetadataAsync(request);
            return new KeyValuePair<string, long>(key, response.ContentLength);
        });
        KeyValuePair<string, long>[]? sizes = await Task.WhenAll(tasks);
        return sizes.ToDictionary();
    }

    /// <inheritdoc/>
    public async Task DeleteFilesAsync(IEnumerable<string> objectKeys)
    {
        DeleteObjectsRequest? deleteObjectsRequest = new()
        {
            BucketName = _storeOption.Bucket,
            Objects = objectKeys.Select(key => new KeyVersion { Key = key }).ToList()
        };
        await _s3Client.DeleteObjectsAsync(deleteObjectsRequest);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 释放资源.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _s3Client.Dispose();
            }

            disposedValue = true;
        }
    }
}