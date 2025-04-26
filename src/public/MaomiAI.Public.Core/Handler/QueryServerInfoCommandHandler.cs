using MaomiAI.Infra;
using MaomiAI.Infra.Services;
using MaomiAI.Public.Queries;
using MaomiAI.Public.Queries.Response;
using MediatR;

namespace MaomiAI.Public.Handler;

public class QueryServerInfoCommandHandler : IRequestHandler<QueryServerInfoCommand, QueryServerInfoResponse>
{
    private readonly SystemOptions _systemOptions;
    private readonly IRsaProvider _rsaProvider;

    public QueryServerInfoCommandHandler(SystemOptions systemOptions, IRsaProvider rsaProvider)
    {
        _systemOptions = systemOptions;
        _rsaProvider = rsaProvider;
    }

    public async Task<QueryServerInfoResponse> Handle(QueryServerInfoCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        return new QueryServerInfoResponse
        {
            PublicStoreUrl = _systemOptions.PublicStore.Endpoint,
            ServiceUrl = _systemOptions.Server,
            RsaPublic = _rsaProvider.GetPublicKey()
        };
    }
}
