using MaomiAI.AiModel.Shared.Helpers;
using MaomiAI.AiModel.Shared.Queries;
using MaomiAI.AiModel.Shared.Queries.Respones;
using MediatR;

namespace MaomiAI.AiModel.Core.Queries;

public class QuerySupportModelProviderCommandHandler : IRequestHandler<QuerySupportModelProviderCommand, QuerySupportModelProviderCommandResponse>
{
    public async Task<QuerySupportModelProviderCommandResponse> Handle(QuerySupportModelProviderCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return new QuerySupportModelProviderCommandResponse
        {
            Providers = AiProviderHelper.Providers
        };
    }
}
