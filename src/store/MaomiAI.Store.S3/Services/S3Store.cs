using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using MaomiAI.Infra;
using static MaomiAI.Infra.SystemOptions;

namespace MaomiAI.Store.Services;

public abstract class S3Store
{
    protected readonly IAmazonS3 _s3Client;
    protected readonly StoreOption _storeOption;

    public S3Store(SystemOptions systemOptions)
    {
        var endpoint = systemOptions.PublicStoreS3.Endpoint;
        endpoint = endpoint.EndsWith('/') ? endpoint[0..(endpoint.Length - 1)] : endpoint;
        _storeOption = new StoreOption
        {
            AccessKeyId = systemOptions.PublicStoreS3.AccessKeyId,
            AccessKeySecret = systemOptions.PublicStoreS3.AccessKeySecret,
            Bucket = systemOptions.PublicStoreS3.Bucket,
            Endpoint = endpoint
        };

        _s3Client = new AmazonS3Client(_storeOption.AccessKeyId, _storeOption.AccessKeySecret, new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USEast1,
            ServiceURL = _storeOption.Endpoint,
            ForcePathStyle = true
        });
    }

    public async Task<string> UploadFileAsync(Stream inputStream, string key)
    {
        using var fileTransferUtility = new TransferUtility(_s3Client);
        await fileTransferUtility.UploadAsync(inputStream, _storeOption.Bucket, key);
        return $"{_storeOption.Endpoint}/{key}";
    }


    public async Task DeleteFileAsync(string key)
    {
        var deleteObjectRequest = new DeleteObjectRequest
        {
            BucketName = _storeOption.Bucket,
            Key = key
        };
        await _s3Client.DeleteObjectAsync(deleteObjectRequest);
    }

    public string GetFileUrl(string key, TimeSpan expiryDuration)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _storeOption.Bucket,
            Key = key,
            Expires = DateTime.UtcNow.Add(expiryDuration)
        };
        return _s3Client.GetPreSignedURL(request);
    }

    public async Task<Credentials> GetTemporarySessionAsync(int durationSeconds)
    {
        using var stsClient = new AmazonSecurityTokenServiceClient();
        var sessionTokenRequest = new GetSessionTokenRequest
        {
            DurationSeconds = durationSeconds
        };
        var sessionTokenResponse = await stsClient.GetSessionTokenAsync(sessionTokenRequest);
        return sessionTokenResponse.Credentials;
    }
}
