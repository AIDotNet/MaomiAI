namespace MaomiAI.Public.Queries.Response;

public class QueryServerInfoResponse
{
    /// <summary>
    /// 系统访问地址.
    /// </summary>
    public string ServiceUrl { get; init; }

    /// <summary>
    /// 公共存储地址，静态资源时可直接访问.
    /// </summary>
    public string PublicStoreUrl { get; init; }

    /// <summary>
    /// RSA 公钥，用于加密密码等信息传输到服务器.
    /// </summary>
    public string RsaPublic { get; init; }
}