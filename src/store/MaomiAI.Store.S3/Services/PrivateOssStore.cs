using MaomiAI.Infra;

namespace MaomiAI.Store.Services;

public class PrivateOssStore : S3Store
{
    public PrivateOssStore(SystemOptions systemOptions) : base(systemOptions)
    {
    }
}
