using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.SecurityToken;
using Amazon.SecurityToken.Model;
using MaomiAI.Infra;
using static MaomiAI.Infra.SystemOptions;

namespace MaomiAI.Store.Services;

public class PublicOssStore : S3Store
{
    public PublicOssStore(SystemOptions systemOptions) : base(systemOptions)
    {
    }
}
