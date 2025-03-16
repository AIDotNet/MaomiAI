using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Net;
using static MaomiAI.Infra.SystemOptions;

namespace MaomiAI.Store.Services;

public class S3Store: IFileStore
{
    protected readonly IAmazonS3 _s3Client;
    protected readonly StoreOption _storeOption;

    public S3Store(StoreOption storeOption)
    {
        _storeOption = storeOption;

        _s3Client = new AmazonS3Client(_storeOption.AccessKeyId, _storeOption.AccessKeySecret, new AmazonS3Config
        {
            ServiceURL = _storeOption.Endpoint,
            ForcePathStyle = true,
            UseHttp = true
        });
    }

    public async Task<Uri> UploadFileAsync(Stream inputStream, string objectKey)
    {
        using var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(inputStream, _storeOption.Bucket, objectKey);
        return new Uri($"{_storeOption.Endpoint}/{objectKey}");
    }

    public async Task<IReadOnlyDictionary<string, Uri>> GetFileUrlAsync(IEnumerable<string> objectKeys, TimeSpan expiryDuration)
    {
        var tasks = objectKeys.Select(async key =>
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _storeOption.Bucket,
                Key = key,
                Expires = DateTime.UtcNow.Add(expiryDuration)
            };
            var url = await _s3Client.GetPreSignedURLAsync(request);
            return new KeyValuePair<string, Uri>(key, new Uri(url));
        });

        var urls = await Task.WhenAll(tasks);

        return urls.ToDictionary();
    }

    public async Task<bool> FileExistsAsync(string objectKey)
    {
        var request = new GetObjectMetadataRequest
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

    public async Task<IReadOnlyDictionary<string, long>> GetFileSizeAsync(IEnumerable<string> objectKeys)
    {
        var tasks = objectKeys.Select(async key =>
        {
            var request = new GetObjectMetadataRequest
            {
                BucketName = _storeOption.Bucket,
                Key = key
            };
            var response = await _s3Client.GetObjectMetadataAsync(request);
            return new KeyValuePair<string, long>(key, response.ContentLength);
        });
        var sizes = await Task.WhenAll(tasks);
        return sizes.ToDictionary();
    }

    public async Task DeleteFilesAsync(IEnumerable<string> objectKeys)
    {
        var deleteObjectsRequest = new DeleteObjectsRequest
        {
            BucketName = _storeOption.Bucket,
            Objects = objectKeys.Select(key => new KeyVersion { Key = key }).ToList()
        };
        await _s3Client.DeleteObjectsAsync(deleteObjectsRequest);
    }
}